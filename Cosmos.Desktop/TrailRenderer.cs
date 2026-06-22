//using Cosmos.Domain.Structs;
//using static Raylib_cs.Raylib;
//using Raylib_cs;

//namespace Cosmos.Desktop;

//public sealed class TrailRenderer
//{
//    private const int CenterX = 640;
//    private const int CenterY = 360;

//    public void Render(
//        Queue<Vector3D> trail,
//        Camera camera)
//    {
//        foreach (var point in trail)
//        {
//            var projectedX =
//    point.X +
//    point.Z * 0.5;

//            var projectedY =
//                point.Y -
//                point.Z * 0.5;

//            DrawCircle(
//                CenterX +
//                (int)((projectedX - camera.Position.X)
//                * camera.Zoom),

//                CenterY +
//                (int)((projectedY - camera.Position.Y)
//                * camera.Zoom),

//                1,

//                Color.DarkGray);
//        }
//    }
//}