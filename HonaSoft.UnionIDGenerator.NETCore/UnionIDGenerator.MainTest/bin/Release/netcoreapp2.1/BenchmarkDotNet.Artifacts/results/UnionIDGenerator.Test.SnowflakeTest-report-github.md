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
|   CreateID10000 |  Clr |     Clr |    19.51 ms | 0.0108 ms | 0.0090 ms |
|  CreateID100000 |  Clr |     Clr |   195.02 ms | 0.1501 ms | 0.1404 ms |
| CreateID1000000 |  Clr |     Clr | 1,952.88 ms | 0.4174 ms | 0.3904 ms |
|   CreateID10000 | Core |    Core |    19.51 ms | 0.0071 ms | 0.0067 ms |
|  CreateID100000 | Core |    Core |   195.03 ms | 0.1576 ms | 0.1474 ms |
| CreateID1000000 | Core |    Core | 1,952.80 ms | 0.3569 ms | 0.3338 ms |
