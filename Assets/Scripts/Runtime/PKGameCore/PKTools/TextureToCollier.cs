// using System.Collections.Generic;
// using UnityEngine;
// using OpenCVForUnity.CoreModule;
// using OpenCVForUnity.ImgprocModule;
// using OpenCVForUnity.UnityUtils;
// using Sirenix.OdinInspector;
// using Unity.VisualScripting;
//
// public class TextureToCollier : MonoBehaviour
// {
//     private Sprite inputSprite;
//
//     [Button("TextureToCollier")]
//     public void ToCollier()
//     {
//         inputSprite = GetComponent<SpriteRenderer>().sprite;
//         if (inputSprite == null)
//         {
//             Debug.LogError("Please assign a sprite to inputSprite!");
//             return;
//         }
//
//         Texture2D inputTexture = new Texture2D((int)inputSprite.rect.width, (int)inputSprite.rect.height);
//         var col = inputSprite.texture.GetPixels((int)inputSprite.rect.x, (int)inputSprite.rect.y,
//             (int)inputSprite.rect.width, (int)inputSprite.rect.height);
//         inputTexture.SetPixels(col);
//         inputTexture.Apply();
//
//         Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);
//         Utils.texture2DToMat(inputTexture, inputMat);
//         Mat binaryMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC1);
//         for (int x = 0; x < inputTexture.width; x++)
//         {
//             for (int y = 0; y < inputTexture.height; y++)
//             {
//                 Color pixel = inputTexture.GetPixel(x, y);
//                 if (pixel.a > 0.1) // Check alpha value
//                 {
//                     binaryMat.put(y, x, 255); // Set pixel to white
//                 }
//                 else
//                 {
//                     binaryMat.put(y, x, 0); // Set pixel to black
//                 }
//             }
//         }
//
//         List<MatOfPoint> contours = new List<MatOfPoint>();
//         Mat hierarchy = new Mat();
//         Imgproc.findContours(binaryMat, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);
//
//         int outerContourIdx = -1;
//         double maxArea = 0;
//         for (int i = 0; i < contours.Count; i++)
//         {
//             double area = Imgproc.contourArea(contours[i]);
//             if (area > maxArea)
//             {
//                 maxArea = area;
//                 outerContourIdx = i;
//             }
//         }
//
//         // 对最外层轮廓进行多边形拟合
//         MatOfPoint2f approxCurve = new MatOfPoint2f();
//         MatOfPoint2f contour2f = new MatOfPoint2f(contours[outerContourIdx].toArray());
//         double epsilon = 0.02 * Imgproc.arcLength(contour2f, true);
//         Imgproc.approxPolyDP(contour2f, approxCurve, epsilon, true);
//
//         // 将多边形顶点坐标转换为Unity坐标系
//         Vector2[] polygonPoints = new Vector2[approxCurve.rows()];
//         for (int i = 0; i < approxCurve.rows(); i++)
//         {
//             double[] point = approxCurve.get(i, 0);
//             var x = (float)point[0] / inputTexture.width - 0.5f;
//             var y = (inputTexture.height - (float)point[1]) / inputTexture.height - 0.5f;
//             Vector3 localPoint = new Vector3(x, y, 0);
//             polygonPoints[i] = new Vector2(localPoint.x, localPoint.y);
//         }
//
//         // 设置多边形顶点给PolygonCollider2D
//         var polygonCollider = GetComponent<PolygonCollider2D>();
//         if (polygonCollider == null)
//         {
//             polygonCollider = this.AddComponent<PolygonCollider2D>();
//         }
//
//         polygonCollider.points = polygonPoints;
//
//         binaryMat.release();
//         inputMat.release();
//     }
// }