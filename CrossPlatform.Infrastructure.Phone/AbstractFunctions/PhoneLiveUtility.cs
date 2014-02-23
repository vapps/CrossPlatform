using CrossPlatform.Infrastructure.Models;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Infrastructure.Phone
{
    public class PhoneLiveUtility : LiveUtility
    {
        private LiveAuthClient authClient;

        public override async Task LoginAsync(IList<string> scopes)
        {
            if (string.IsNullOrEmpty(ClientId))
            {
                EtcUtility.Instance.MsgBox("ClientId Required");
                return;
            }

            if (AuthClient == null)
                AuthClient = authClient = new LiveAuthClient(ClientId);
            else
                authClient = (LiveAuthClient)AuthClient;

            try
            {
                IsBusy = true;
                await TaskEx.Run(() => 
                {
                    StaticFunctions.InvokeIfRequiredAsync(StaticFunctions.BaseContext,
                        para =>
                        {
                            //authClient.LoginCompleted +=
                            //    (s, e) =>
                            //    {
                            //        if (e.Status == LiveConnectSessionStatus.Connected)
                            //        {
                            //            LoadUser();
                            //        }
                            //    };
                            //두번째 접속 시도
                            authClient.LoginAsync(scopes);

                        }, null);
                });
            }
            catch (Exception ex)
            {
                EtcUtility.Instance.MsgBox("NotConnectLiveID " + ex.Message);
            }
        }

        private void LoadUser()
        {
#if WindowsPhone8
            //윈도우 자체에 로그인 되어있으면 로그아웃 앙보여줘도 괜찮은..그렇지 않은 경우는 보여줌
            Windows.Security.Authentication.OnlineId.OnlineIdAuthenticator aut = new Windows.Security.Authentication.OnlineId.OnlineIdAuthenticator();
            IsSignOut = aut.CanSignOut;
#else
            IsSignOutPossable = true;
#endif

            //프로필 로드하고
            LiveConnectClient client = new LiveConnectClient(authClient.Session);
            //client.GetCompleted +=
            //    (s, e) =>
            //    {
            //        User = Newtonsoft.Json.JsonConvert.DeserializeObject<LiveUser>(e.RawResult);
            //        IsSignIn = true;
            //        IsBusy = false;
            //    };
            client.GetAsync("me");
        }

        public override bool Logout()
        {
            if (string.IsNullOrEmpty(ClientId))
            {
                EtcUtility.Instance.MsgBox("ClientId Required");
                return false;
            }

            var returnValue = false;
            if (AuthClient == null)
                AuthClient = authClient = new LiveAuthClient(ClientId);
            else
                authClient = (LiveAuthClient)AuthClient;

            authClient.Logout();

            IsSignIn = false;
            User = null;

            returnValue = true;

            return returnValue;
        }


        public override Task InitializeAsync(IList<string> scopes)
        {
            throw new NotImplementedException();
        }
    }
}
