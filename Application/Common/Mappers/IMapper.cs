using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mapper
{
    public interface IMapper
    {
        TargetType Map<TargetType>(object source);
        TargetType Map<SourceType, TargetType>(SourceType instance);
        TargetType Merge<SourceType, TargetType>(SourceType source, TargetType target);
    }
}
