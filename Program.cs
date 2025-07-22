using OpenCvSharp;
using Sdcb.OpenVINO.PaddleOCR;
using Sdcb.OpenVINO.PaddleOCR.Models;
using Sdcb.OpenVINO.PaddleOCR.Models.Online;
using System.Diagnostics;

namespace PaddleOCRApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("PaddleOCR V4 演示程序");
            Console.WriteLine("====================");
            Console.WriteLine();

            try
            {
                // 下载并初始化模型
                Console.WriteLine("正在下载 PaddleOCR V4 模型...");
                FullOcrModel model = await OnlineFullModels.ChineseV4.DownloadAsync();
                Console.WriteLine("模型下载完成！");
                Console.WriteLine();

                // 创建PaddleOCR实例
                using PaddleOcrAll ocr = new(model)
                {
                    AllowRotateDetection = true,
                    Enable180Classification = true,
                };

                // 如果用户提供了图片路径参数
                if (args.Length > 0 && File.Exists(args[0]))
                {
                    Console.WriteLine($"正在识别图片: {args[0]}");
                    
                    var image = Cv2.ImRead(args[0]);
                    if (!image.Empty())
                    {
                        var stopwatch = Stopwatch.StartNew();
                        var result = ocr.Run(image);
                        stopwatch.Stop();
                        
                        Console.WriteLine($"识别完成，用时: {stopwatch.ElapsedMilliseconds} 毫秒");
                        Console.WriteLine();
                        DisplayResults(result);
                        
                        image.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("无法加载图片文件");
                    }
                }
                else
                {
                    Console.WriteLine("使用方法：");
                    Console.WriteLine("dotnet run <图片路径>");
                    Console.WriteLine();
                    Console.WriteLine("例如：dotnet run test.jpg");
                    Console.WriteLine();
                    
                    // 创建一个简单的测试图片（如果没有提供参数）
                    Console.WriteLine("创建测试图片进行演示...");
                    var testImage = CreateTestImage();
                    
                    var stopwatch = Stopwatch.StartNew();
                    var result = ocr.Run(testImage);
                    stopwatch.Stop();
                    
                    Console.WriteLine($"测试图片识别完成，用时: {stopwatch.ElapsedMilliseconds} 毫秒");
                    Console.WriteLine();
                    DisplayResults(result);
                    
                    testImage.Dispose();
                }

                Console.WriteLine();
                Console.WriteLine("演示完成！按任意键退出...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Console.WriteLine("请检查网络连接和系统环境");
                Console.WriteLine();
                Console.WriteLine("按任意键退出...");
                Console.ReadKey();
            }
        }

        private static void DisplayResults(PaddleOcrResult result)
        {
            if (result == null)
            {
                Console.WriteLine("识别结果为空");
                return;
            }

            try
            {
                // 显示完整文本
                Console.WriteLine("=== 识别结果 ===");
                Console.WriteLine(result.Text ?? "未识别到文字内容");
                Console.WriteLine();

                // 尝试显示详细信息
                if (result.Regions != null)
                {
                    var regions = result.Regions.ToList();
                    Console.WriteLine($"识别到 {regions.Count} 个文本区域：");
                    Console.WriteLine();

                    for (int i = 0; i < regions.Count; i++)
                    {
                        var region = regions[i];
                        Console.WriteLine($"[{i + 1}] 文本：{region.Text}");
                        Console.WriteLine($"    置信度：{region.Score:F4}");
                        
                        try
                        {
                            // 显示区域信息
                            Console.WriteLine($"    中心点：({region.Rect.Center.X:F1}, {region.Rect.Center.Y:F1})");
                            Console.WriteLine($"    尺寸：{region.Rect.Size.Width:F1} x {region.Rect.Size.Height:F1}");
                            Console.WriteLine($"    角度：{region.Rect.Angle:F1}°");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"    位置信息获取失败：{ex.Message}");
                        }
                        
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"显示结果时出错：{ex.Message}");
                Console.WriteLine("但识别可能已完成。");
            }
        }

        private static Mat CreateTestImage()
        {
            // 创建一个简单的白色背景图片，上面写一些文字
            var image = new Mat(300, 600, MatType.CV_8UC3, Scalar.White);
            
            // 添加一些文字作为测试内容
            Cv2.PutText(image, "PaddleOCR V4 Test", new Point(50, 80), 
                HersheyFonts.HersheySimplex, 2, Scalar.Black, 3);
            Cv2.PutText(image, "Hello World!", new Point(100, 150), 
                HersheyFonts.HersheySimplex, 1.5, Scalar.Blue, 2);
            Cv2.PutText(image, "Testing OCR", new Point(150, 220), 
                HersheyFonts.HersheySimplex, 1.2, Scalar.Red, 2);
            
            return image;
        }
    }
}