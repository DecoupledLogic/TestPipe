namespace TestPipe.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IListElement : IElement
    {
        ReadOnlyCollection<IElement> GetList(string selectedElement); //{ get; }
    }
}
