# AntoriGameNetwork

CSharpServer : ThreadPool, Async/Await 형태의 C# 서버 입니다. 이 프로젝트의 메인입니다.

ProtocolGenerator : ProtoBuf와 비슷한 기능(IDL 컴파일러)을 하는 프로토콜 생성기 입니다. C#은 잘 생성되고, C++은 작업중입니다. 지원 언어는 계속 추가 될 예정입니다.

Client : 테스트 용 C# 클라이언트 입니다.

RestAPIServer : ASP.NET Core 기반 REST API 서버입니다. 기반 작업만 되어있습니다.

CppServer : IOCP 기반 C++ 서버 입니다. (미완성)

프로젝트 예정 목록
1. DLL화를 통해 C++, C# 라이브러리 공유
2. 운영툴(ASP.net Core Blazor)
3. Pub/Sub(Go)
4. Server Code Editor(TypeScript)
