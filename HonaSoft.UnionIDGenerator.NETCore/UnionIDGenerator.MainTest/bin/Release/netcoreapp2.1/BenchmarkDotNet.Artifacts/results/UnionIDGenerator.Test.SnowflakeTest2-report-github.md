``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.345 (1803/April2018Update/Redstone4)
Intel Core i5-8400 CPU 2.80GHz (Max: 2.81GHz) (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=2.1.400
  [Host] : .NET Core 2.1.2 (CoreCLR 4.6.26628.05, CoreFX 4.6.26629.01), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3190.0
  Core   : .NET Core 2.1.2 (CoreCLR 4.6.26628.05, CoreFX 4.6.26629.01), 64bit RyuJIT


```
|          Method |  Job | Runtime |        Mean |     Error |    StdDev |
|---------------- |----- |-------- |------------:|----------:|----------:|
|   CreateID10000 |  Clr |     Clr |    19.51 ms | 0.0088 ms | 0.0078 ms |
|  CreateID100000 |  Clr |     Clr |   195.01 ms | 0.1684 ms | 0.1575 ms |
| CreateID1000000 |  Clr |     Clr | 1,952.64 ms | 0.3207 ms | 0.2843 ms |
|   CreateID10000 | Core |    Core |    19.51 ms | 0.0108 ms | 0.0101 ms |
|  CreateID100000 | Core |    Core |   195.04 ms | 0.1535 ms | 0.1436 ms |
| CreateID1000000 | Core |    Core | 1,952.72 ms | 0.3094 ms | 0.2895 ms |
