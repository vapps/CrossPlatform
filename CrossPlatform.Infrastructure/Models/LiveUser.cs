using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Models
{
    public class LiveUser : BindableBase
    {
        private string id;
        /// <summary>
        /// The user's ID.
        /// </summary>
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string name;
        /// <summary>
        /// The user's full name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string firstName;
        /// <summary>
        /// The user's first name.
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        private string lastName;
        /// <summary>
        /// The user's last name.
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        private string link;
        /// <summary>
        /// The URL of the user's profile page. wl.basic
        /// </summary>
        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                OnPropertyChanged();
            }
        }

        private DateTime birthday;
        /// <summary>
        /// birth_day, birth_month, birth_year 3개를 합쳐서 생일 생성해야함 wl.birthday
        /// </summary>
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged();
            }
        }

        //work 생략

        private string gender;
        /// <summary>
        /// The user's gender. Valid values are "male", "female", or null if the user's gender is not specified.
        /// </summary>
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged();
            }
        }

        private string locale;
        /// <summary>
        /// The user's locale code.
        /// </summary>
        public string Locale
        {
            get { return locale; }
            set
            {
                locale = value;
                OnPropertyChanged();
            }
        }

        private string updatedTime;
        /// <summary>
        /// The time, in ISO 8601 format, at which the user last updated the object.
        /// </summary>
        public string UpdatedTime
        {
            get { return updatedTime; }
            set
            {
                updatedTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 사진 uri
        /// </summary>
        public Uri PictureUri 
        {
            get 
            {
                var returnValue = string.Empty;
                if (Id.Length > 0)
                    returnValue = string.Format("https://cid-{0}.users.storage.live.com/users/0x{0}/myprofile/expressionprofile/profilephoto:UserTileStatic", Id);
                else
                    returnValue = "about:blank";
                return new Uri(returnValue);
            }
        }
    }
}
