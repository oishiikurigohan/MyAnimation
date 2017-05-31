using System;
using System.Reflection;
using System.Collections.Generic;
using CocosSharp;

namespace MyAnimation
{
    public class GameScene : CCScene
    {
        private CCAnimate animate;
        private CCSprite sprite;
        private CCRepeatForever walkRepeat;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            // CCSceneにCCLayerColorをセット( CCLayerColor = CCLayer + 色情報 )
            var layer = new CCLayerColor(CCColor4B.White);
            AddLayer(layer);

            // このアセンブリのすべてのリソースIDを取得
            var assembly = typeof(GameScene).GetTypeInfo().Assembly;
            string[] FileNames = assembly.GetManifestResourceNames();

            // 3桁以上の連番が入っているPNGをアニメーション用のファイルとする。
            // アニメーションで使う画像ファイルは、表示する順番に3桁の連番.pngで作成し
            // MyAnimation(PCL)/Resources/Images/Framesに入れてあります。
            var spriteframes = new List<CCSpriteFrame>();
            var rect = new CCRect(0, 0, 300, 100);
            foreach (String FileName in FileNames)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(FileName, @"\w*\d{3,}\w*\.png$"))
                {
                    // 後の処理で、レイヤーにセットするCCSpriteを生成する際に
                    // アニメーションが構築されたCCSpriteFrameリストの1フレームを渡すため
                    // 埋め込みリソースファイルをStreamで取得し、CCTexture2Dを生成しCCSpriteFrameを作っている。
                    // 
                    // ちなみにCCRectは画像ファイルのサイズ情報( 横300px、縦100p× )。
                    // streamから画像ファイルのサイズ情報をGetしようと思ったけど
                    // 簡単ではなさそう＆そこまで頑張る必要がないと判断しとりあえず固定。
                    // 
                    // やってることがおかしかったら教えてください。。。
                    var stream = assembly.GetManifestResourceStream(FileName);
                    var texture2d = new CCTexture2D(stream, CCSurfaceFormat.Color);
                    spriteframes.Add(new CCSpriteFrame(texture2d, rect));
                }
            }

            // CCRepeatForever : 親 CCAction : CCNodeにセットされると指定されたCCActionを永遠に繰り返す。
            // CCAnimate       : 親 CCAction : セットされたCCAnimationに入っている画像を1つ1つ表示することができる。
            // CCAnimation     : 親 Object   : CCSpriteでCCAnimationをアニメ化するにはCCAnimateを使用する。
            // CCSprite        : 親 CCNode   : 2D画像をレイヤーに貼り付けるためのもの。
            var animation = new CCAnimation(spriteframes, 0.1f);
            animate = new CCAnimate(animation);
            walkRepeat = new CCRepeatForever(animate);

            // アニメーションの1フレームをセットしてSpriteを生成し、レイヤの真ん中に設置する。
            // 何フレーム目をセットしても、配列の先頭から再生されます。そのへんの制御はCCAnimateなのかしら
            sprite = new CCSprite(spriteframes[20]);
            sprite.Position = new CCPoint(layer.ContentSize.Center.X, layer.ContentSize.Center.Y);
            layer.AddChild(sprite);

            // Spriteアニメーションをフォーエヴァーリピートしてみる
            var state = sprite.RunAction(walkRepeat);

            // フォーエヴァーでなく回数指定の場合はこれ
            // animation.Loops = 5;
            // var state = sprite.RunAction(animate);
        }

        public void FastRewindAnimation()
        {
            System.Diagnostics.Debug.WriteLine("倍速逆再生をタップしました");
        }

        public void RewindAnimation()
        {
            System.Diagnostics.Debug.WriteLine("逆再生をタップしました");
        }

        public void StopAnimation()
        {
            System.Diagnostics.Debug.WriteLine("停止をタップしました");
            // ブログ用のスクリーンショットを撮るために一時的にこうしてるけどあとで直すと思う
            sprite.Pause();
        }

        public void PlayAnimation()
        {
            System.Diagnostics.Debug.WriteLine("再生をタップしました");
        }

        public void FastFowardAnimation()
        {
            System.Diagnostics.Debug.WriteLine("倍速再生をタップしました");
        }
    }
}
