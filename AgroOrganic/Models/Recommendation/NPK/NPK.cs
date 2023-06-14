using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroOrganic.Models.Recommendation {

  public class NPK {
    public static NPK Default = new NPK(0, 0, 0);
    public double N, P, K;

    public NPK(double N, double P, double K) {
      this.N = N;
      this.P = P;
      this.K = K;
    }

    public static NPK operator *(NPK a, NPK b) => new NPK(a.N * b.N, a.P * b.P, a.K * b.K);
    public static NPK operator *(NPK a, double b) => new NPK(a.N * b, a.P * b, a.K * b);
    public static NPK operator *(NPK a, int b) => new NPK(a.N * b, a.P * b, a.K * b);

    public static NPK operator /(NPK a, NPK b) => new NPK(a.N / b.N, a.P / b.P, a.K / b.K);
    public static NPK operator /(NPK a, double b) => new NPK(a.N / b, a.P / b, a.K / b);
    public static NPK operator /(NPK a, int b) => new NPK(a.N / b, a.P / b, a.K / b);

    public static NPK operator +(NPK a, NPK b) => new NPK(a.N + b.N, a.P + b.P, a.K + b.K);
    public static NPK operator +(NPK a, double b) => new NPK(a.N + b, a.P + b, a.K + b);
    public static NPK operator +(NPK a, int b) => new NPK(a.N + b, a.P + b, a.K + b);

    public static NPK operator -(NPK a, NPK b) => new NPK(a.N - b.N, a.P - b.P, a.K - b.K);
    public static NPK operator -(NPK a, double b) => new NPK(a.N - b, a.P - b, a.K - b);
    public static NPK operator -(NPK a, int b) => new NPK(a.N - b, a.P - b, a.K - b);

    public override string ToString() {
      return "NPK(" + Math.Round(N, 3) + ", " + Math.Round(P, 3) + ", " + Math.Round(K, 3) + ")";
    }
  }
}