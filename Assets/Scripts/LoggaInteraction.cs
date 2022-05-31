using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoggaInteraction : IInputInteraction
{
    public float Duration = 0.2f;

    [UnityEditor.InitializeOnLoadMethod]
    private static void Register()
    {
        InputSystem.RegisterInteraction<LoggaInteraction>();
    }

    public void Process(ref InputInteractionContext context)
    {
        if (context.timerHasExpired)
        {
            context.Canceled();
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                if (context.ReadValue<float>() == 1)
                {
                    context.Started();
                    context.SetTimeout(Duration);
                }
                break;
            case InputActionPhase.Started:
                if (context.ReadValue<float>() == -1)
                {
                    context.Performed();
                }
                break;
        }
    }

    public void Reset()
    {
    }
}
