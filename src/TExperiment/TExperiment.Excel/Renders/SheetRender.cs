using System.Linq;
using TExperiment.Excel.Descriptor;
using TExperiment.Excel.Models;

namespace TExperiment.Excel.Renders
{
    public class SheetRender:Render
    {
        private readonly SheetDescriptor _sheetDescriptor;


        public SheetRender(SheetDescriptor sheetDescriptor)
        {
            _sheetDescriptor = sheetDescriptor;

            var repeatStartParameterDescriptors = _sheetDescriptor.ParameterDescriptors.Where(p => p.Value.StartsWith("#")).ToList();
            var repeatEndParameterDescriptors = _sheetDescriptor.ParameterDescriptors.Where(p => p.Value.StartsWith("/")).ToList();

            foreach(var start in repeatStartParameterDescriptors)
            {
                var valueArr = start.Value.Substring(1).Split(':');
                var name = valueArr[0];
                var end = repeatEndParameterDescriptors.SingleOrDefault(p => p.Value.Substring(1).Equals(name));
                if (end != null)
                {
                    var repeatRender = new RepeatRender()
                    {
                        Name = name,
                        StartCell = start.Cell,
                        EndCell = end.Cell,
                        Repeat = valueArr[1].Equals("x") ? RepeatType.X : RepeatType.Y,
                    };

                    Childs.Add(repeatRender);
                }
            }
        }
    }
}
