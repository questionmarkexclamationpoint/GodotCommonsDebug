using Godot;
using Commons;
using System;

//namespace Game;

[Tool]
public partial class RandomizeDebug : Node2D {
    int pointCount = 10;
    [Export]
    int PointCount {
        get => pointCount;
        set {
            pointCount = value;
            QueueRedraw();
        }
    }

    float minValue = -1;
    [Export]
    float MinValue {
        get => minValue;
        set {
            minValue = value;
            QueueRedraw();
        }
    }

    float maxValue = 1;
    [Export]
    float MaxValue {
        get => maxValue;
        set {
            maxValue = value;
            QueueRedraw();
        }
    }

    bool useIntegers = false;
    [Export]
    bool UseIntegers {
        get => useIntegers;
        set {
            useIntegers = value;
            QueueRedraw();
        }
    }

    public override void _Draw() {
        DrawLine(new(MinValue, 0), new(MaxValue, 0), Colors.Black, 0.01f);
        for (int i = 0; i < PointCount; i++) {
            var value = UseIntegers
                ? Randomize.RandomRangeInt((int)Math.Ceiling(MinValue), (int)Math.Floor(MaxValue))
                : Randomize.RandomRange(MinValue, MaxValue);
            DrawCircle(new(value, 0), 0.025f, Colors.Black);
        }
    }
}
