#if false

using System;
using static MyPhys.PhysMath;

namespace MyPhys
{
    public static class Collisions
    {
        // 分離軸定理を使用する?
        // separating axis theorem (SAT)
        // エピソード03 円の衝突判定

        // 真であれば交差している
        public static bool IntersectCircles(
            Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out Vector2 normal, out float depth)
        {
            // パラメーターにデフォルト値を指定する
            normal = Vector2.Zero;
            depth = 0f;

            // using staticしているから省略できる
            float distance = Distance(centerA, centerB);
            float radii = radiusA + radiusB;

            if (distance >= radii)  // 衝突していない falseを返す
            {
                return false;
            }

            // 衝突している場合
            normal = Normalize(centerB - centerA);
            depth = radii - distance;

            return true;
        }

    }
}

#endif