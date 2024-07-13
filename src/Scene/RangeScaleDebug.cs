using Godot;
using System;
using Range = QuestionMarkExclamationPoint.Commons.Range;

[Tool]
public partial class RangeScaleDebug : Node2D {
    private float value = 0;
    [Export]
    public float Value {
        get => value;
        set {
            this.value = value;
            QueueRedraw();
        }
    }

    private float inMin = -1;
    [Export]
    public float InMin {
        get => inMin;
        set {
            inMin = value;
            QueueRedraw();
        }
    }

    private float inMid = 0;
    [Export]
    public float InMid {
        get => inMid;
        set {
            inMid = value;
            QueueRedraw();
        }
    }

    private float inMax = 1;
    [Export]
    public float InMax {
        get => inMax;
        set {
            inMax = value;
            QueueRedraw();
        }
    }

    private float outMin = -5;
    [Export]
    public float OutMin {
        get => outMin;
        set {
            outMin = value;
            QueueRedraw();
        }
    }

    private float outMid = 5;
    [Export]
    public float OutMid {
        get => outMid;
        set {
            outMid = value;
            QueueRedraw();
        }
    }

    private float outMax = 15;
    [Export]
    public float OutMax {
        get => outMax;
        set {
            outMax = value;
            QueueRedraw();
        }
    }

    public override void _Draw() {

        DrawLine(new(InMin, 1), new(InMid, 1), Colors.Red, 0.05f);
        DrawLine(new(InMid, 1), new(InMax, 1), Colors.Blue, 0.05f);

        DrawLine(new(OutMin, -1), new(OutMid, -1), Colors.Red, 0.05f);
        DrawLine(new(OutMid, -1), new(OutMax, -1), Colors.Blue, 0.05f);

        DrawArc(new(Value, 1), 0.1f, 0, 2 * (float)Math.PI, 32, Colors.Black, 0.1f);
        var result = Compute(Value);
        DrawArc(new(result, -1), 0.1f, 0, 2 * (float)Math.PI, 32, Colors.Black, 0.1f);
        DrawLine(new(Value, 1), new(result, -1), Colors.Black, 0.05f);

    }

    private float Compute(float value) => Range.Map(value, (InMin, InMid, InMax), (OutMin, OutMid, OutMax));
}
