using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TExperiment.Excel.Descriptor;
using TExperiment.Excel.Models;
using TExperiment.Excel.Renders;

namespace TExperiment.Excel.Builders
{
    public class RenderBuilder
    {
        private const string RepeatStartPattern = "#";
        private const string RepeatEndPattern = "/";
        private const string RepeatTypeSeparator = ":";
        private const string RepeatXString = "x";
        private const string NameSeparator = ".";
        private const string FilterPattern = @"\[([^\]]+)\]";
        private const string FilterOperator = "=";

        public IList<IRender> Build(IEnumerable<SheetDescriptor> sheetDescriptors)
        {
            var result = new List<IRender>();
            foreach (var sheetDescriptior in sheetDescriptors)
            {
                var sheetRender = new SheetRender()
                {
                    Name = sheetDescriptior.Name
                };
                sheetRender.Childs = Build(sheetDescriptior.ParameterDescriptors);
                result.Add(sheetRender);
            }

            return result;
        }

        private IList<IRender> Build(IEnumerable<ParameterDescriptor> parameters)
        {
            var repeatStartParameters = parameters.Where(p => p.Value.StartsWith(RepeatStartPattern)).ToList();
            var repeatEndParameters = parameters.Where(p => p.Value.StartsWith(RepeatEndPattern)).ToList();
            var cellParameters =
                parameters.Where(p => !(p.Value.StartsWith(RepeatStartPattern) || p.Value.StartsWith(RepeatEndPattern)))
                    .ToList();

            var repeatRenders = new List<RepeatRender>();
            foreach (var start in repeatStartParameters)
            {
                var valueArr = start.Value.Substring(1).Split(RepeatTypeSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (valueArr.Length == 0)
                {
                    continue;
                }

                var name = valueArr[0];
                var end = repeatEndParameters.SingleOrDefault(p => p.Value.Substring(1).Equals(name));
                if (end != null)
                {
                    var repeatRender = new RepeatRender()
                    {
                        Name = name,
                        Range = new CellRange()
                        {
                            StartCell = start.Cell,
                            EndCell = end.Cell,
                        },
                    };
                    repeatRender.Repeat=RepeatType.Y;
                    if (valueArr.Length >= 2)
                    {
                        repeatRender.Repeat = valueArr[1].Equals(RepeatXString) ? RepeatType.X : RepeatType.Y;
                    }

                    repeatRenders.Add(repeatRender);
                }
            }

            var cellRenders = new List<CellRender>();
            foreach (var parameter in cellParameters)
            {
                var render = BuildCellRender(parameter);
                if (render != null)
                {
                    cellRenders.Add(render);
                }
            }

            return SortAndOrganize(repeatRenders, cellRenders);
        }

        private IList<IRender> SortAndOrganize(IEnumerable<RepeatRender> repeatRenders,
            IEnumerable<CellRender> cellRenders)
        {
            var sortedRepeatRenders =
                repeatRenders.OrderBy(r => r.Repeat)
                    .ThenByDescending(r => r.Range.EndCell.ColumnIndex)
                    .ThenByDescending(r => r.Range.EndCell.RowIndex)
                    .ToList();
            var sortedCellRenders =
                cellRenders.OrderByDescending(r => r.Cell.ColumnIndex).ThenByDescending(r => r.Cell.RowIndex);
            var organizeRepeatRenders = new List<RepeatRender>();

            var result = new List<IRender>();
            foreach (var repeatRender in sortedRepeatRenders)
            {
                if (repeatRender.Repeat == RepeatType.Y)
                {
                    var crossRepeatRender =
                        organizeRepeatRenders.SingleOrDefault(
                            r => r.Repeat == RepeatType.X && r.Range.HasCross(repeatRender.Range));
                    if (crossRepeatRender != null)
                    {
                        crossRepeatRender.Childs.Add(repeatRender);
                    }
                    else
                    {
                        organizeRepeatRenders.Add(repeatRender);
                        result.Add(repeatRender);
                    }
                }
                else
                {
                    organizeRepeatRenders.Add(repeatRender);
                    result.Add(repeatRender);
                }
            }
            foreach (var cellRender in sortedCellRenders)
            {
                var range =
                    repeatRenders.Where(r => r.Range.Include(cellRender.Cell)).OrderByDescending(r => r.Repeat).FirstOrDefault();
                if (range != null)
                {
                    range.Childs.Add(cellRender);
                }
                else
                {
                    result.Add(cellRender);
                }
            }


            return result;
        }

        private CellRender BuildCellRender(ParameterDescriptor parameter)
        {
            var nameArr = parameter.Value.Split(NameSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (nameArr.Length == 0)
            {
                return null;
            }

            CellRender render = null;
            CellRender parentRender = null;
            CellRender currentRender = null;
            string name;
            for (var i = 0; i < nameArr.Length; i++)
            {
                name = nameArr[i];
                if (i == nameArr.Length - 1)
                {
                    currentRender = new CellRender();
                }
                else
                {
                    currentRender = new CellNodeRender()
                    {
                        Filters = BuildFilters(ref name),
                    };
                }
                currentRender.Cell = parameter.Cell;
                currentRender.Name = name;

                if (i == 0)
                {
                    render = currentRender;
                }
                if (parentRender != null)
                {
                    parentRender.Childs.Add(currentRender);
                }
                parentRender = currentRender;
            }

            return render;
        }

        private IList<FilterCondition> BuildFilters(ref string name)
        {
            var result = new List<FilterCondition>();
            var matches = Regex.Matches(name, FilterPattern);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    for (var i = 1; i < match.Groups.Count; i++)
                    {
                        var filterArr = match.Groups[i].Value.Split(FilterOperator.ToCharArray(),
                            StringSplitOptions.RemoveEmptyEntries);
                        if (filterArr.Length == 2)
                        {
                            var filter = new FilterCondition()
                            {
                                Name = filterArr[0],
                            };

                            var compareNameArr=filterArr[1].Split(RepeatTypeSeparator.ToCharArray(),
                            StringSplitOptions.RemoveEmptyEntries);
                            filter.CompareName = compareNameArr[0];
                            filter.Repeat = RepeatType.Y;
                            if (compareNameArr.Length >= 2)
                            {
                                filter.Repeat = compareNameArr[1].Equals(RepeatXString) ? RepeatType.X : RepeatType.Y;
                            }

                            result.Add(filter);
                        }
                    }
                }
            }
            name = Regex.Replace(name, FilterPattern, "");

            return result;
        }
    }
}
