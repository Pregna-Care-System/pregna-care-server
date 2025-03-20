//using QuestPDF.Fluent;
//using QuestPDF.Infrastructure;
//using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
//using QuestPDF.Helpers;
//using SkiaSharp;

//namespace PregnaCare.Documents.Components
//{
//    public class RevenueChartComponent : IComponent
//    {
//        private readonly List<RevenueStatsResponse> _revenueData;

//        public RevenueChartComponent(List<RevenueStatsResponse> revenueData)
//        {
//            _revenueData = revenueData;
//        }

//        public void Compose(IContainer container)
//        {
//            container.Column(column =>
//            {
//                column.Item().Text("Monthly Revenue").Bold().FontSize(16);

//                foreach (var revenue in _revenueData)
//                {
//                    column.Item().Text($"Year: {revenue.Year}").Bold();
//                    column.Item().Canvas((canvas, size) =>
//                    {
//                        var maxRevenue = revenue.TotalRevenueByMonth.Max();
//                        var barWidth = size.Width / 12;

//                        // Tạo một SKCanvas từ canvas của QuestPDF
//                        var skCanvas = canvas;

//                        // Tạo một SKPaint để vẽ hình chữ nhật
//                        var paint = new SKPaint
//                        {
//                            Color = SKColors.Blue,
//                            IsAntialias = true,
//                            Style = SKPaintStyle.Fill
//                        };

//                        for (int i = 0; i < 12; i++)
//                        {
//                            var barHeight = (float)((revenue.TotalRevenueByMonth[i] / maxRevenue) * size.Height * 0.8);

//                            // Vẽ hình chữ nhật bằng SKCanvas
//                            skCanvas(
//                                new SKRect(
//                                    i * barWidth,
//                                    size.Height - barHeight,
//                                    (i + 1) * barWidth - 5,
//                                    size.Height
//                                ),
//                                paint
//                            );

//                            // Thêm nhãn tháng
//                            canvas.DrawText($"M{i + 1}")
//                                .FontSize(10)
//                                .FontColor(SKColors.Black)
//                                .At(i * barWidth + 5, size.Height - 20);
//                        }
//                    });
//                }
//            });
//        }
//    }
//}