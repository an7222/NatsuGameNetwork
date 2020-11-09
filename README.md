# AntoriGameNetwork

CSharpServer : ThreadPool, TAP 형태의 C# 서버 입니다. 이 프로젝트의 메인입니다.

ProtocolGenerator : ProtoBuf와 비슷한 기능(IDL 컴파일러)을 하는 프로토콜 생성기 입니다. C#은 잘 생성되고, C++은 작업중입니다. 지원 언어는 계속 추가 될 예정입니다.

Client : 테스트 용 C# 클라이언트 입니다.

RestAPIServer : ASP.NET core 기반 REST API 서버입니다. 기반 작업만 되어있습니다.

CppServer : IOCP 기반 C++ 서버 입니다. (미완성)


1. 클래스 간의 라이프 사이클이 명확하도록, 클래스 생성 관리를 하나의 클래스가 담당하도록 노력하였습니다.

2. 클래스 간의 연결고리 중 상속이 가장 나중에 끊기가 어려운 관계이기 때문에, 부모클래스에는 최소한의 동작만 들어가도록 하였습니다.

3. 멤버 함수의 대부분이 재정의가 필요한경우 인터페이스나 가상클래스를 이용하도록 추구하였습니다.


프로젝트 예정 목록
1. DLL화를 통해 C++, C# 라이브러리 공유
2. 운영툴(C# Blazor)
3. Pub/Sub(Go)
4. Server Code Editor(TypeScript)
