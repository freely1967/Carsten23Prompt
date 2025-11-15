using ApocalypticFastFood;

// Simplified Program bootstrap. To run demo scenarios, pass the argument "samples" or set the
// environment variable RUN_SAMPLES=1. Demo code is moved to Samples.SampleRunner.
var runSamples = args.Contains("samples") || Environment.GetEnvironmentVariable("RUN_SAMPLES") == "1";

Console.WriteLine("App started. Use 'samples' argument to run demo scenarios.");

if (runSamples)
{
    Samples.SampleRunner.RunDemos();
}