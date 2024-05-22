using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourAppNamespace;

namespace LoginDesign3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private FirebaseHelper _firebaseHelper = new FirebaseHelper();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            var user = await _firebaseHelper.GetUserByUsernameAndPassword(username, password);

            if (user != null)
            {
                await DisplayAlert("로그인 성공", "환영합니다!", "OK");
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("로그인 실패", "아이디 또는 비밀번호가 잘못되었습니다.", "OK");
            }
        }

        private async void JoinButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JoinPage());
        }
    }
}
