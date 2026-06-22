//using static Raylib_cs.Raylib;
//using Raylib_cs;

//namespace Cosmos.Desktop;

//public sealed class HudRenderer
//{
//    public void Render(
//        Camera camera,
//        int simulationSpeed,
//        bool paused)
//    {
//        DrawText(
//            $"Zoom: {camera.Zoom:F2}",
//            10,
//            10,
//            20,
//            Color.Green);

//        DrawText(
//            $"Speed: {simulationSpeed}",
//            10,
//            40,
//            20,
//            Color.Green);

//        DrawText(
//            $"Paused: {paused}",
//            10,
//            70,
//            20,
//            Color.Green);

//        DrawText(
//            $"Target: {camera.Target?.Name}",
//            10,
//            100,
//            20,
//            Color.Green);
//    }
//}