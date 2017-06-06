using System;
using System.Reflection;
using System.Collections.Generic;
using CocosSharp;

namespace MyAnimation
{
    public class GameScene : CCScene
    {
        private CCSprite sprite;
        private CCActionState state;
        private SortSpriteFrames sortspriteframes;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            var layer = new CCLayerColor(CCColor4B.White);
            AddLayer(layer);

            // このアセンブリのすべてのリソースIDを取得
            var assembly = typeof(GameScene).GetTypeInfo().Assembly;
            string[] FileNames = assembly.GetManifestResourceNames();

            // 3桁以上の連番が入っているPNGをアニメーション用のファイルとする。
            // 画像とIDを紐づけるためのリスト(spriteframesInfo)を作成する。
            var spriteframesInfo = new List<SpriteFrameInfo>();
            var rect = new CCRect(0, 0, 300, 100);
            foreach (String FileName in FileNames)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(FileName, @"\w*\d{3,}\w*\.png$"))
                {
                    var stream = assembly.GetManifestResourceStream(FileName);
                    var texture2d = new CCTexture2D(stream, CCSurfaceFormat.Color);
                    var spriteframe = new CCSpriteFrame(texture2d, rect);
                    spriteframesInfo.Add(new SpriteFrameInfo(spriteframe));
                }
            }
            sortspriteframes = new SortSpriteFrames(spriteframesInfo);

            // spriteをnewするときに渡したフレームが初期画像となります。
            var sorted_spriteframes = sortspriteframes.GetSortedSpriteFrames();
            sprite = new CCSprite(sorted_spriteframes[0]);
            sprite.Position = new CCPoint(layer.ContentSize.Center.X, layer.ContentSize.Center.Y);
            layer.AddChild(sprite);
        }

        // 倍速逆再生イメージをタップしたときの処理 
        public void FastRewindAnimation()
        {
            sprite.StopAction(state);
            var reversesorted_spriteframes = sortspriteframes.GetReverseSortedSpriteFrames(sprite);
            var animate = new CCAnimate(new CCAnimation(reversesorted_spriteframes, 0.1f));
            animate.Duration = 1;
            state = sprite.RepeatForever(animate);
        }

        // 逆再生イメージをタップしたときの処理
        public void RewindAnimation()
        {
            sprite.StopAction(state);
            var reversesorted_spriteframes = sortspriteframes.GetReverseSortedSpriteFrames(sprite);
            var animate = new CCAnimate(new CCAnimation(reversesorted_spriteframes, 0.1f));
            animate.Duration = 2;
            state = sprite.RepeatForever(animate);
        }

        // 停止イメージをタップしたときの処理
        public void StopAnimation() { sprite.StopAction(state); }

        // 再生イメージをタップしたときの処理
        public void PlayAnimation()
        {
            sprite.StopAction(state);
            var sorted_spriteframes = sortspriteframes.GetSortedSpriteFrames(sprite);
            var animate = new CCAnimate(new CCAnimation(sorted_spriteframes, 0.1f));
            animate.Duration = 2;
            state = sprite.RepeatForever(animate);
        }

        // 倍速再生イメージをタップしたときの処理
        public void FastFowardAnimation()
        {
            sprite.StopAction(state);
            var sorted_spriteframes = sortspriteframes.GetSortedSpriteFrames(sprite);
            var animate = new CCAnimate(new CCAnimation(sorted_spriteframes, 0.1f));
            animate.Duration = 1;
            state = sprite.RepeatForever(animate);
        }
    }
}
