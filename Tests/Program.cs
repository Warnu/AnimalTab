using System;
using System.Reflection;
using System.Runtime.Loader;

var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(System.IO.Path.GetFullPath(args[0]));
var type = assembly.GetType("AnimalTab.GatherableStatus", true);
var read = (Func<MethodInfo, object, int, Action<Exception>, bool>)type
    .GetMethod("Read", BindingFlags.Static | BindingFlags.NonPublic)
    .CreateDelegate(typeof(Func<MethodInfo, object, int, Action<Exception>, bool>));
var getter = typeof(StatusComponent).GetProperty(nameof(StatusComponent.Active)).GetMethod;
int passed = 0, warnings = 0;
Action<Exception> warn = exception => { Check(exception is NullReferenceException, "original null-reference diagnosis retained"); warnings++; };
var healthy = new StatusComponent();
Check(read(getter, healthy, 0, warn), "healthy active component stays active");
healthy.Eligible = false;
Check(!read(getter, healthy, 1, warn), "healthy inactive component stays inactive");
Check(!read(getter, null, 2, warn), "missing component is inactive");

var broken = new StatusComponent { Parent = null };
try { getter.Invoke(broken, null); throw new Exception("Expected the original failure"); }
catch (TargetInvocationException e) when (e.InnerException is NullReferenceException) { passed++; }
broken.Calls = 0;
Check(!read(getter, broken, 100, warn), "original reflected null-reference is contained");
for (int i = 101; i < 1100; i++) Check(!read(getter, broken, i, warn), "failed cell stays blank during cooldown");
Check(broken.Calls == 1 && warnings == 1, "repeated draws neither invoke nor log again during cooldown");
Check(!read(getter, broken, 1100, warn) && broken.Calls == 2 && warnings == 1, "retry occurs once per second without repeated warnings");
broken.Parent = new ParentState();
Check(read(getter, broken, 2100, warn), "recovered component resumes normal display");
Check(read(getter, broken, 2101, warn), "successful reads are not throttled");
broken.Parent = null;
Check(!read(getter, broken, 2102, warn) && warnings == 1, "later failure remains contained without log spam");

healthy.Eligible = true;
Check(read(getter, healthy, 2103, warn), "failure does not affect another animal");
var second = new StatusComponent { Parent = null };
Check(!read(getter, second, int.MaxValue - 500, warn), "second component gets independent failure state");
int before = second.Calls;
Check(!read(getter, second, int.MinValue + 100, warn) && second.Calls == before, "cooldown survives TickCount rollover");
second.Parent = new ParentState();
Check(read(getter, second, int.MinValue + 500, warn), "retry works after TickCount rollover");

var otherFailure = new StatusComponent { OtherError = true };
try { read(getter, otherFailure, 0, warn); throw new Exception("An unrelated error was swallowed"); }
catch (TargetInvocationException e) when (e.InnerException is InvalidOperationException) { passed++; }
try { read(null, healthy, 0, warn); throw new Exception("A missing getter was swallowed"); }
catch (ArgumentNullException) { passed++; }
Console.WriteLine($"PASS: {passed} checks against the compiled AnimalTab.dll (including 999 repeated redraws).");

void Check(bool condition, string message) { if (!condition) throw new Exception("FAIL: " + message); passed++; }
public sealed class ParentState { public bool Valid = true; }
public sealed class StatusComponent {
    public ParentState Parent = new ParentState();
    public bool Eligible = true, OtherError;
    public int Calls;
    public bool Active { get { Calls++; if (OtherError) throw new InvalidOperationException("Unrelated error"); return Parent.Valid && Eligible; } }
}
