using System;
using System.Collections.Generic;
using System.Text;

namespace GitSync
{
    public class Parameter
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int RootId { get; set; }
        public bool HasChild { get; set; }
        public string Title { get; set; }
        public string  Description { get; set; }
        public string TagName { get; set; }
        public string ACriteriaName { get; set; }
        public string GWT { get; set; }

    }
}
