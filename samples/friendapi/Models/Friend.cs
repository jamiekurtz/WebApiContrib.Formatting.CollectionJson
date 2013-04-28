using System;

namespace WebApiContrib.Formatting.CollectionJson.Models
{
    public class Friend
    {
        private string _fullName;

        public int Id { get; set; }
        public string ShortName { get; private set; }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                string tempName = _fullName.ToLower();
                ShortName = tempName.Substring(0, 1) + tempName.Substring(tempName.IndexOf(" ", StringComparison.Ordinal) + 1);
            }
        }

        public string Email { get; set; }
        public Uri Blog { get; set; }
        public Uri Avatar { get; set; }
    }
}