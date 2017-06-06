using System.Collections.Generic;
using System.Reflection;
using CocosSharp;

namespace MyAnimation
{
    // 画像とIDを紐づけるためのクラス
    public class SpriteFrameInfo
    {
        public CCSpriteFrame SpriteFrame { get; }
        public int TextureId { get; }

        public SpriteFrameInfo(CCSpriteFrame spriteframe)
        {
            // privateメンバ spriteFrame.Texture.textureId の値をGetする
            this.SpriteFrame = spriteframe;
            var fieldinfo = spriteframe.Texture.GetType().GetTypeInfo().GetDeclaredField("textureId");
            this.TextureId = (int)fieldinfo.GetValue(spriteframe.Texture);
        }
    }

    public class SortSpriteFrames
    {
        static private List<SpriteFrameInfo> SpriteFrameInfo;

        public SortSpriteFrames(List<SpriteFrameInfo> spriteframeinfo)
        {
            SortSpriteFrames.SpriteFrameInfo = spriteframeinfo;
        }

        // 引数で指定されたフレーム画像の次を先頭とするフレームリストを返却する
        public List<CCSpriteFrame> GetSortedSpriteFrames(CCSprite sprite = null)
        {
            int IndexNo = 0;
            var sorted_spriteframe = new List<CCSpriteFrame>();
            if (sprite != null)
            {
                // 現在spriteに表示されているTextureのtextureIdを利用し、次に表示する画像の要素番号(IndexNo)を取得する
                var fieldinfo = sprite.Texture.GetType().GetTypeInfo().GetDeclaredField("textureId");
                int CurrentTextureId = (int)fieldinfo.GetValue(sprite.Texture);
                IndexNo = SpriteFrameInfo.FindIndex(item => item.TextureId == CurrentTextureId) + 1;
            }

            // System.Diagnostics.Debug.WriteLine("まず[" + IndexNo + "] から [" + (SpriteFrameInfo.Count - 1) + "] までAddする。");
            for (int i = IndexNo; i < SpriteFrameInfo.Count; i++) sorted_spriteframe.Add(SpriteFrameInfo[i].SpriteFrame);
            int SubtractCount = SpriteFrameInfo.Count - sorted_spriteframe.Count;
            // System.Diagnostics.Debug.WriteLine("次に[0]から " + SubtractCount + "個Addする。");
            for (int i = 0; i < SubtractCount; i++) sorted_spriteframe.Add(SpriteFrameInfo[i].SpriteFrame);

            return sorted_spriteframe;
        }

        // 引数で指定されたフレーム画像の前を先頭とする逆順のフレームリストを返却する
        public List<CCSpriteFrame> GetReverseSortedSpriteFrames(CCSprite sprite = null)
        {
            int IndexNo = SpriteFrameInfo.Count - 1;
            var sorted_spriteframe = new List<CCSpriteFrame>();
            if (sprite != null)
            {
                // 現在spriteに表示されているTextureのtextureIdを利用し、次に表示する画像の要素番号(IndexNo)を取得する
                var fieldinfo = sprite.Texture.GetType().GetTypeInfo().GetDeclaredField("textureId");
                int CurrentTextureId = (int)fieldinfo.GetValue(sprite.Texture);
                IndexNo = SpriteFrameInfo.FindIndex(item => item.TextureId == CurrentTextureId) - 1;
            }

            // System.Diagnostics.Debug.WriteLine("まず[" + IndexNo + "] から [0] まで逆順でAddする。");
            for (int i = IndexNo; i >= 0;  i--) { sorted_spriteframe.Add(SpriteFrameInfo[i].SpriteFrame); }
            int SubtractCount = sorted_spriteframe.Count;
            // System.Diagnostics.Debug.WriteLine("次に[" + (SpriteFrameInfo.Count - 1) + "]から[" + SubtractCount + "]まで逆順にAddする。");
            for (int i = SpriteFrameInfo.Count - 1; i >= SubtractCount; i--) sorted_spriteframe.Add(SpriteFrameInfo[i].SpriteFrame);

            return sorted_spriteframe;
        }
    }
}
