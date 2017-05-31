using CocosSharp;
using Xamarin.Forms;

namespace MyAnimation
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            GameScene gameScene = null;
            var cocossharpView = new CocosSharpView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ViewCreated = (sender, e) =>
                {
                    var ccgameView = sender as CCGameView;
                    ccgameView.DesignResolution = new CCSizeI(300, 200);
                    gameScene = new GameScene(ccgameView);
                    ccgameView.RunWithScene(gameScene);
                }
            };
            MyGrid.Children.Add(cocossharpView, 0, 0);

            // 倍速逆再生画像をタップしたときの処理
            var FastRewindImageTap = new TapGestureRecognizer();
            FastRewindImageTap.Tapped += (sender, e) => gameScene.FastRewindAnimation();
            FastRewindImage.GestureRecognizers.Add(FastRewindImageTap);

            // 逆再生画像をタップしたときの処理
            var RewindImageTap = new TapGestureRecognizer();
            RewindImageTap.Tapped += (sender, e) => gameScene.RewindAnimation();
            RewindImage.GestureRecognizers.Add(RewindImageTap);

            // 停止画像をタップしたときの処理
            var StopImageTap = new TapGestureRecognizer();
            StopImageTap.Tapped += (sender, e) => gameScene.StopAnimation();
            StopImage.GestureRecognizers.Add(StopImageTap);

            // 再生画像をタップしたときの処理
            var PlayImageTap = new TapGestureRecognizer();
            PlayImageTap.Tapped += (sender, e) => gameScene.PlayAnimation();
            PlayImage.GestureRecognizers.Add(PlayImageTap);

            // 倍速再生画像をタップしたときの処理
            var FastFowardImageTap = new TapGestureRecognizer();
            FastFowardImageTap.Tapped += (sender, e) => gameScene.FastFowardAnimation();
            FastFowardImage.GestureRecognizers.Add(FastFowardImageTap);
        }
    }
}
