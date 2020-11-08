#pragma once

#include <iostream>
#include <string.h>
#include <process.h>
#include <Winsock2.h>
#include <vector>

#define PORT        8001
#define BUFSIZE     1024
#define READ        3
#define WRITE       5

#pragma comment(lib,"ws2_32.lib")

using namespace std;

///////////////////////////////////////////////
// Structure Definition
typedef struct          // 소켓 정보를 구조체화
{
    SOCKET      clientSocket;
    SOCKADDR_IN clntAddr;
} PER_HANDLE_DATA, * LPPER_HANDLE_DATA;

typedef struct          // 소켓의 버퍼 정보를 구조체화
{
    OVERLAPPED overlapped;
    CHAR       buffer[BUFSIZE];
    WSABUF     wsaBuf;
    int        rwMode;
} PER_IO_DATA, * LPPER_IO_DATA;


////////////////////////////////////////////////
// Class definition
class IOCPServer {
public:
    IOCPServer();
    ~IOCPServer();

    bool Run();

    static unsigned int __stdcall _CompletionThread(void* p_this);
    UINT WINAPI CompletionThread();

private:
    HANDLE  CompletionPortHandler;

    SOCKET listenSocket;
    SOCKADDR_IN serverAddr;
    vector<SOCKET> socketList;
};