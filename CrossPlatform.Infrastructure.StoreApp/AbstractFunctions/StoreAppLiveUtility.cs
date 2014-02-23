using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossPlatform.Infrastructure.Models;

namespace CrossPlatform.Infrastructure.StoreApp
{
    public class StoreAppLiveUtility : LiveUtility
    {
        LiveAuthClient authClient;

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public override async Task LoginAsync(IList<string> scopes)
        {
            //AuthClient는 프로퍼티
            if (AuthClient == null)
                AuthClient = authClient = new LiveAuthClient();
            else
                authClient = (LiveAuthClient)AuthClient;
              
            LiveLoginResult authResult = null;
            try
            {
                authResult = await authClient.LoginAsync(scopes);
                if (authResult != null && authResult.Status == LiveConnectSessionStatus.Connected)
                {
                    await LoadUser();
                }
            }
            catch (Exception ex)
            {
                EtcUtility.Instance.MsgBox("Not connected LiveID " + ex.Message);
            }
        }

        private async Task LoadUser()
        {
            //윈도우 자체에 로그인 되어있으면 로그아웃 앙보여줘도 괜찮은..그렇지 않은 경우는 보여줌
            Windows.Security.Authentication.OnlineId.OnlineIdAuthenticator aut = new Windows.Security.Authentication.OnlineId.OnlineIdAuthenticator();
            IsSignOutPossable = aut.CanSignOut;

            IsBusy = true;
            //프로필 로드하고
            LiveConnectClient client = new LiveConnectClient(authClient.Session);
            LiveOperationResult liveOpResult = await client.GetAsync("me");
            User = Newtonsoft.Json.JsonConvert.DeserializeObject<LiveUser>(liveOpResult.RawResult);
            IsSignIn = true;
            IsBusy = false;
        }

        public override bool Logout()
        {
            var returnValue = false;
            LiveAuthClient authClient;
            if (AuthClient == null)
                AuthClient = authClient = new LiveAuthClient();
            else
                authClient = (LiveAuthClient)AuthClient;

            authClient.Logout();
            IsSignIn = false;
            User = null;
            returnValue = true;

            return returnValue;
        }

        public override async Task InitializeAsync(IList<string> scopes)
        {
            //AuthClient는 프로퍼티
            if (AuthClient == null)
                AuthClient = authClient = new LiveAuthClient();
            else
                authClient = (LiveAuthClient)AuthClient;

            LiveLoginResult authResult = null;
            try
            {
                authResult = await authClient.InitializeAsync(scopes);
                if (authResult != null && authResult.Status == LiveConnectSessionStatus.Connected)
                {
                    await LoadUser();
                }
            }
            catch (Exception ex)
            {
                EtcUtility.Instance.MsgBox("Not connected LiveID " + ex.Message);
            }
        }
    }
}
