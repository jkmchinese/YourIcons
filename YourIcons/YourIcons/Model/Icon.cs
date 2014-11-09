using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourIcons.Model
{
    public class Icon : IconBase
    {
        public override object Clone()
        {
            return new Icon()
            {
                Name = Name,
                Width = Width,
                Height = Height,
                Data = Data,
                Keyword = Keyword,
                CreatedDataTime = CreatedDataTime,
                ModifiedDataTime = ModifiedDataTime,
                IsFavourite = IsFavourite
            };
        }
    }
}
