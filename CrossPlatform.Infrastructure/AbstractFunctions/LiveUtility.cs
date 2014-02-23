using CrossPlatform.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure
{
    public abstract class LiveUtility : BindableBase
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static LiveUtility Instance { get; set; }

        /// <summary>
        /// 초기화(한번 로그인된 사용자의 정보를 다시 가지고 오는 듯?) - 앱이 시작하자마자 시도?
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public abstract Task InitializeAsync(IList<string> scopes);

        /// <summary>
        /// Live ID 로그인
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public abstract Task LoginAsync(IList<string> scopes);

        /// <summary>
        /// Live Id 로그아웃
        /// </summary>
        /// <returns></returns>
        public abstract bool Logout();

        private bool isBusy;
        /// <summary>
        /// busy
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }
        
        private bool _isSignIn;
        /// <summary>
        /// 사인인 여부
        /// </summary>
        public bool IsSignIn
        {
            get { return _isSignIn; }
            set { SetProperty(ref _isSignIn, value); }
        }

        private bool _isSignOutPossable;
        /// <summary>
        /// 사인아웃 가능 여부
        /// </summary>
        public bool IsSignOutPossable
        {
            get { return _isSignOutPossable; }
            set { SetProperty(ref _isSignOutPossable, value); }
        }

        /// <summary>
        /// AuthClient Instance
        /// </summary>
        public object AuthClient { get; set; }

        private LiveUser user;
        /// <summary>
        /// 로그인 사용자 정보
        /// </summary>
        public LiveUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Phone에서는 필요함
        /// </summary>
        public string ClientId { get; set; }
    }
}
