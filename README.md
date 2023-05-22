# LamLibAllOver

Simple help lib

[![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://GitHub.com/Naereen/StrapDown.js/graphs/commit-activity)
[![NuGet stable version](https://badgen.net/nuget/v/LamLibAllOver)](https://www.nuget.org/packages/LamLibAllOver)
[![GitHub license](https://badgen.net/github/license/Naereen/Strapdown.js)](https://github.com/chaosmac1/LamLibAllOver/blob/master/LICENSE)

![Magic](https://img.shields.io/badge/Lambda-blue?style=for-the-badge&logo=data:image/webp;base64,UklGRl4EAABXRUJQVlA4WAoAAAAQAAAAMQAAMQAAQUxQSI0BAAABkCzJtmlb89pq2fc+tl7L9vsD27Zt27ZtWy3bts2N8bDPnnOueB8QERNA0kFkfOaZkaaFH8RZw8LWAtfNitsK4IRRyecB4HCGQbHn4Hkv2Jj4GfDeEGRKtY8IaM9sXs6MSu8CAXBWxJlQ54Mv4GKyXlML3K1BWk0tsJ1MnZixFgQ7aMQufwtRt4RC1i8IP1nTMF6KCve9JgPg84rq4TLU470YgEcTSgYJhLyDrntnXsMiMWH+qMojnX9d61s7fyNhZFc/QfW+GjEryE8zFwZ+6BpMfo9C/8ukDPJ/xID3kcTspvfxYBgn15K798XjWxJx20J+23eP7/GsZQrf4PkkiHVczN56zeswGYNOp71WGfThl1d/VtRLucC1WI2g7uSxdum9D2Vd0ztO7KN6U3kL9OrzzukVYxWy1T5FslpD/XQQa6Ka05HYfZV+fupM/NKOyobioSS5R2NTCMkmLLbE9kaTeLnN30Xutw0lzYSaw9dceu8Gcl7uH1QxggyMSC9aqnbt2rVrVU8Oo/8lAFZQOCCqAgAA8A0AnQEqMgAyAD6RQplKJaOiIagYCqiwEglAGaW1cRPbd3bNPkv0AeLH9NkQOwIMmVSjKfGb9VewJ0dvRGWtrcTrMzDJeHmsaI9mzAhEE3wTKyjXU1eKMx8xZpDqHdOsZx+uRrllb26DxoC9wEp8dYOSNditkY+wAP50GP/xOAj9MxiNkhiyBKDilAWOXj5EGk2tZf6ZqlKYhYzksEARHMXe6a0yKFm91i4m9TmWXg2e6ypHH9/HWTPM5gxISn2SFSlsQ9mHk2kS59akT7qE2r7jajlUX3V0bVMQuoRc+a1ndfz/JRapEeucUWy9nLNKK57MNZ22W06/vrfeC6WOZgPOo7H7E1+KtJlf6adPbRf/pHn3SdWauyqc/742kc6/AiSn+pSF3M/8+YZ8u4R4iD3miL669gSs8gwqeAp6J/7LzX4k6LaLi9Tvg0FqVevuzKZ3dUFaNbWqrSswDzA5wCzlb1x6GZW1sphYDVCp942YoqiORbpXuIRZQ11+DjYpEArrGzeZ49oLulxaEYMI99NX4VxBHlzHWIeEwqf/Nnrj3mZ80nai1607Ga3wKfnNPEt1gIl3SiKnB8FEpCHrEzHKWPu5vXDIfV7wanqUhev/5orqrhLDJAfoXdMBGTtxvxkbAzkfceAf2G81++WficNd4y2zJH77xd/8kEv//6cSROG4lU+U7wlXElWQAogaiCr7/Qjj0iBuUwV9D9UJrLfpXo0Y7JPtyCxWADvQWq649vR3ySi7UA+naKBVL5Kb5qhM9ONqSDBiAERpAvjSdwhIl7WU2Y3ZCWbF/NE/qjyBhM/7/nCB49WtrXSf613k9dNNpJ6X0y2gXH93dTNNYIlTKErJUpijLwdMoRBX6yPYn48hZv8cvgv3EvWDCM/erjA/+bk29iAAAA==)

# Box

Explicit Readonly Box For Structs

```csharp
var f = new Box(new A());

struct A() {}
```

# Result

Light Result Pattern Like Rust.

## Create Result
```csharp
var resultErr = Result<int, string>.Err("Error Stack ...");
var resultOk = Result<int, string>.Ok(42);
```
## Check Status
```csharp
var resultErr = Result<int, string>.Err("Error Stack ...");
if (resultErr == EResult.Err) {
    // Error Case
}
```
## Get Ok
```csharp
var result = Result<int, string>.Ok(42);
var num = result.Ok();
```
## Get Err
```csharp
var resultErr = Result<int, string>.Err("Error Stack ...");
var str = resultErr.Err();
```
## Get OkOr
```csharp
var resultErr = Result<int, string>.Err("Error Stack ...");
var num = resultErr.OkOr(1337); 
// num == 1337
```
## Get ErrOr
```csharp
var result = Result<int, string>.Ok(42);
var str = result.ErrOr("TEXT");
// str == "TEXT"
```
## Get OkOrDefault
```csharp
var resultErr = Result<int, string>.Err("Error Stack ...");
var num = resultErr.OkOrDefault(); 
// num == 0
```
## Get ErrOrDefault
```csharp
var result = Result<int, string>.Ok(42);
var str = result.ErrOrDefault();
// str == null
```
## AndThen
```csharp
var num = Result<int, string>
  .Ok(2)
  .AndThen(x => Result<int, string>.Ok(x + 1))
  .AndThen(x => Result<int, string>.Ok(x + 1))
  .Ok();
// num == 4
```

```csharp
var res = Result<int, string>
  .Ok(2)
  .AndThen(x => Result<int, string>.Ok(x + 1))
  .AndThen(x => Result<int, string>.Err("ERR"))
  .AndThen(x => Result<int, string>.Ok(x + 1))
  .Err();
// res == "ERR"
```
## And
````csharp

static Result<object, string> Fetch<T>(string url, out T) { ... }

string v1;
string v2;
var res = Fetch("URL1", out v1).And(Fetch("URL2", out v2));
if (res == EResult.ERR) {
   // Err
}
````
## Map

## MapErr

## AndThenAsync

## AndAsync

## MapAsync

## MapErrAsync

## Empty

## ToNone

## implicit operator

## ChangeOkType

## ChangeErrType

## Out

# ResultErr

## Create ResultErr

## Check Status

## Unwrap

## Get Err

## Get ErrOr

## Get ErrOrDefault

## AndThen

## AndThenAsync

## And

## AndAsync

## MapErr

## MapErrAsync

## Map

## MapAsync

## Empty

## ToResultWithErr

## ToNone

## ConvertTo

## Out

# ResultOk

## Create ResultOk

## Check Status

## Unwrap

## Get Ok

## Get OkOr

## Get OkOrDefault

## AndThen

## And

## Map

## AndThenAsync

## AndAsync

## MapAsync

## Empty

## ToNone

# ResultOpen

## Create ResultOpen

## Check Status

# ResultNone

## Create ResultNone

## Check Status

## Unwrap

## ThrowIfErr

## And

## AndAsync

# Option

## Create Option

## Check Status

## Unwrap

## Or

## OrNull

## Empty

## With

## implicit operator

## Trim

## ResultWrapper

## Transform

## Map

## MapAsync

## NullSplit

## IteratorAny

## IteratorAnyAsync

# Sha3

# SqlIn

# Time

# TraceMsg

# TryCatch

# ULongFunc

# Convert

## StringToByteArray

## ByteArrayToString

## Base64Decode

## Base64DecodeString

## Base64EncoderString

## ReadFully

# DateUuid

# Iter