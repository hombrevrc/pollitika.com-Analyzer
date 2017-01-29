using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pollitika.com_Analyzer
{
    public class User
    {
        private string _name;
        private string _nameHtml;
        private string _memberSince;
        private List<Post> _listPosts;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string NameHtml
        {
            get { return _nameHtml; }
            set { _nameHtml = value; }
        }

        public string MemberSince
        {
            get { return _memberSince; }
            set { _memberSince = value; }
        }
    }
}
