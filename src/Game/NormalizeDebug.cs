using Godot;
using System;
using Commons;

//namespace Game;

[Tool]
public partial class NormalizeDebug : Node2D {
    private NormalizationStrategy strategy = NormalizationStrategy.TANH;
    [Export]
    public NormalizationStrategy Strategy {
        get => strategy;
        set {
            strategy = value;
            QueueRedraw();
        }
    }

    private int pointCount = 25;
    [Export]
    public int PointCount {
        get => pointCount;
        set {
            pointCount = value;
            QueueRedraw();
        }
    }
    private float outMin = -1;
    [Export]
    public float OutMin {
        get => outMin;
        set {
            outMin = value;
            QueueRedraw();
        }
    }
    private float outMax = 1;
    [Export]
    public float OutMax {
        get => outMax;
        set {
            outMax = value;
            QueueRedraw();
        }
    }
    private float squeeze = 1;
    [Export]
    public float Squeeze {
        get => squeeze;
        set {
            squeeze = value;
            QueueRedraw();
        }
    }
    private float shift;
    [Export]
    public float Shift {
        get => shift;
        set {
            shift = value;
            QueueRedraw();
        }
    }
    private bool clamp = false;
    [Export]
    public bool Clamp {
        get => clamp;
        set {
            clamp = value;
            QueueRedraw();
        }
    }
    private float testValue = 0;
    [Export]
    public float TestValue {
        get => testValue;
        set {
            testValue = value;
            QueueRedraw();
        }
    }

    public override void _Draw() {
        DrawLimits();
        DrawNonNormalizedLine();
        DrawNormalizedLine();
        DrawPoint();
    }

    private void DrawLimits() {
        // Left line
        DrawLine(new(-1, -OutMin), new(-1, -OutMax), Colors.Black, 0.025f);
        // Top line
        DrawLine(new(-1, -OutMax), new(1, -OutMax), Colors.Black, 0.025f);
        // Right line
        DrawLine(new(1, -OutMin), new(1, -OutMax), Colors.Black, 0.025f);
        // Bottom line
        DrawLine(new(-1, -OutMin), new(1, -OutMin), Colors.Black, 0.025f);
    }

    private void DrawNonNormalizedLine() {
        // Diagonal line
        DrawLine(new Vector2(-1, -OutMin), new Vector2(1, -OutMax), Colors.Red, 0.025f);
    }

    private void DrawNormalizedLine() {
        // TODO cluster points around origin more tightly
        int externalPointCount = (int)Math.Floor(PointCount / 4f);
        DrawPolyline(RangePoints(-2, -1, externalPointCount + 1), Colors.DarkGray, 0.05f);
        DrawPolyline(RangePoints(-1, 1, PointCount - externalPointCount + 1), Colors.White, 0.05f);
        DrawPolyline(RangePoints(2, 1, externalPointCount + 1), Colors.DarkGray, 0.05f);
    }

    private void DrawPoint() {
        // Test Point
        Vector2 point = NormalizedResult(TestValue);
        DrawCircle(point, 0.05f, Colors.Blue);
        DrawSetTransform(new(1, -OutMin), 0, new Vector2(1 / Scale.X, 1 / Scale.Y));
        DrawString(ThemeDB.FallbackFont, Vector2.Right * 14, $"({point.X}, {-point.Y})", HorizontalAlignment.Fill, fontSize: 10, modulate: Colors.Black);
    }

    private Vector2[] RangePoints(float start, float end, int count) {
        float range = end - start;
        float distance = range / (count - 1); // TODO something wrong here lol
        Vector2[] result = new Vector2[count];
        for (int i = 0; i < count; i++) {
            float x = start + distance * i;
            result[i] = NormalizedResult(x);
        }
        return result;
    }

    private Vector2 NormalizedResult(float x) {
        float y = Strategy.Apply(x, min: OutMin, max: OutMax, squeeze: Squeeze, shift: Shift, clamp: Clamp);
        return new(x, -y);
    }
}

public delegate float Normalizer(
            float value,
            float min = -1,
            float max = 1,
            float shift = 0,
            float squeeze = 0,
            bool clamp = false
);

public enum NormalizationStrategy {
    TANH,
    SIGMOID
}

public static class NormalizationStrategyExtensions {
    public static float Apply(this NormalizationStrategy name,
            float value,
            float min = -1,
            float max = 1,
            float shift = 0,
            float squeeze = 0,
            bool clamp = false
    ) {
        return name.Normalizer()(value, min, max, shift, squeeze, clamp);
    }

    private static Normalizer Normalizer(this NormalizationStrategy name) {
        return name switch {
            NormalizationStrategy.TANH => Normalize.Tanh,
            NormalizationStrategy.SIGMOID => Normalize.Sigmoid,
            _ => throw new NotImplementedException()
        };
    }
}
