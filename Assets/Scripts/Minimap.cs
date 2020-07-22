using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Minimap : MonoBehaviour
{
    private Image m_MiniMapImage;           //미니맵 이미지
    private Image m_CoverImage;          //판때기 이미지
    private Canvas m_Canvas;                //미니맵이 그려질 캔버스
    private double m_Aspect;                //비율

    public Sprite sprite;
    public Sprite cover;
    public string fileName;

    public Image coverImage;
    public Image baseImage;
    public RectTransform rect;
    public Texture2D coverTexture;
    public string path;
    public string mapImagePath;

    private void Start()
    {
        path = Path.Combine(Application.dataPath,"Boar.png");
        baseImage = Resources.Load<Image>(mapImagePath);
        coverTexture = new Texture2D(2, 2);
        coverTexture.LoadImage(File.ReadAllBytes(path));

        coverImage = GetComponent<Image>();
        coverTexture.SetPixel(9, 10, new Color(1f, 1f, 0f, 0.5f));
        coverTexture.SetPixel(10, 10, new Color(1f, 1f, 0f, 0.5f));
        coverTexture.SetPixel(9, 11, new Color(1f, 1f, 0f, 0.5f));
        coverTexture.SetPixel(10, 11, new Color(1f, 1f, 0f, 0.5f));
        coverTexture.Apply();

        coverImage.sprite = Sprite.Create(coverTexture, new Rect(0, 0, coverTexture.width, coverTexture.height), new Vector2(0.0f, 0.0f));
        File.WriteAllBytes(path, coverTexture.EncodeToPNG());
    }

    private void Update()
    {
        //Invoke("InitializeUI", 5);
    }


    /// <summary>
    /// 지도 밝히기
    /// </summary>
    /// <param name="x"> 밝힐 위치 X좌표 </param>
    /// <param name="y"> 밝힐 위치 Y좌표 </param>
    /// <param name="radious"> 크기 얼마만하게 밝힐지 </param>
    public void RevealMap(double x, double y, double radious)
    {
        //비율 계산
        int conv_x = (int)(x / m_Aspect);
        int conv_y = (int)(y / m_Aspect);

        //적용
        Texture2D writeableBitmap = coverTexture;
        SetPixcelsInCircle(ref writeableBitmap, conv_x, conv_y, (int)radious, new Color(0f, 0f, 0f, 0f)); //알파값 0으로줘서 해당범위 투명하게 해줌
        coverTexture.Apply();

        coverTexture = writeableBitmap;

        coverImage.sprite = Sprite.Create(coverTexture, new Rect(0, 0, coverTexture.width, coverTexture.height), new Vector2(0.0f, 0.0f));
        File.WriteAllBytes(path, coverTexture.EncodeToPNG());
    }


    /// <summary>
    ///동그라미안의 픽셀 값들 변경하기
    ///유니티에서는 WriteableBitmap 대신 Texture2D로 바꿔서 써라
    /// </summary>
    /// <param name="tex">텍스쳐</param>
    /// <param name="x">X 좌표</param>
    /// <param name="y">Y 좌표</param>
    /// <param name="radious">반지름</param>
    /// <param name="color">색</param>
    /// <returns>수정된 텍스쳐</returns>
    public void SetPixcelsInCircle(ref Texture2D bmp, int x, int y, int radious, Color color)
    {
        //픽셀 돌면서 원형 내에 속하는 픽셀만 수정해줌
        //속하는 거 확인하는 검사는 중점과 임의의 픽셀점의 거리가 반지름보다 길면 원을 벗어나는 것이므로
        //점과 점사이의 거리공식 쓰면댐

        //[최적화 전] 
        //모든 픽셀을 검사해버림. 미니맵이 커버리면 ㅈ댐
        //for (int i = 0; i < bmp.PixelHeight; i++)
        //{
        //    for (int j = 0; j < bmp.PixelWidth; j++)
        //    {
        //        if (Math.Sqrt(Math.Pow(x - j, 2.0) + Math.Pow(y - i, 2.0)) < radious)
        //        {
        //            SetPixel(ref bmp, j, i, color);
        //        }
        //    }
        //}

        //[최적화 후]
        //반지름 * 2의 길이를 가진 사각형내에만 검사
        //그 이상의 최적화는 솔직히 외부 라이브러리 써야할 듯..
/*        int squareLength = radious * 2;
        Rect rect = new Rect(x - radious, y - radious, squareLength, squareLength);
        for (int i = rect.Y < 0 ? 0 : (int)rect.Y; i < bmp.PixelHeight && i < rect.Y + rect.Height; i++)
        {
            for (int j = rect.X < 0 ? 0 : (int)rect.X; j < bmp.PixelWidth && j < rect.X + rect.Width; j++)
            {
                if (Math.Sqrt(Math.Pow(x - j, 2.0) + Math.Pow(y - i, 2.0)) < radious)
                {
                    SetPixel(ref bmp, j, i, color);
                }
            }
        }*/
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(100, 100);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
