using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Anything
{
    public class DrawingCanvas :Canvas

    {
        private List<Visual> visuals = new List<Visual>();
        protected override int VisualChildrenCount
        {
            get
            {
                return base.VisualChildrenCount;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            return base.GetVisualChild(index);
        }
        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);

        }
        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);

        }
        
    }
}
