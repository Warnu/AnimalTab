using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AnimalTab {
    // A status getter can traverse an invalid map/holder while the table is drawing.
    // Isolate that failure to this component, preserving normal game eligibility rules.
    internal static class GatherableStatus {
        private const int RetryDelayMilliseconds = 1000;
        private static readonly ConditionalWeakTable<object, FailureState> Failures =
            new ConditionalWeakTable<object, FailureState>();

        private sealed class FailureState {
            public bool Failed;
            public bool Warned;
            public int LastFailure;
        }

        internal static bool Read(MethodInfo getter, object component, int now, Action<Exception> warn) {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            if (component == null) return false;

            Failures.TryGetValue(component, out FailureState state);
            // TickCount subtraction remains valid across its signed integer rollover.
            if (state != null && state.Failed &&
                unchecked((uint)(now - state.LastFailure)) < RetryDelayMilliseconds) return false;

            try {
                bool active = (bool)getter.Invoke(component, null);
                if (state != null) state.Failed = false;
                return active;
            } catch (TargetInvocationException exception) when (exception.InnerException is NullReferenceException) {
                state = state ?? Failures.GetValue(component, _ => new FailureState());
                state.Failed = true;
                state.LastFailure = now;
                if (!state.Warned) {
                    state.Warned = true;
                    warn(exception.InnerException);
                }
                return false;
            }
        }
    }
}
