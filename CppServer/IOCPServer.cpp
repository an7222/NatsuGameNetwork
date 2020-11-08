#include "IOCPServer.h"

IOCPServer::IOCPServer()
{
	WSAData wsaData;
	SYSTEM_INFO systemInfo;

	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
		throw runtime_error("WSAStartup() error!");
	}

	CompletionPortHandler = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, 0, 0);
	GetSystemInfo(&systemInfo);

	for (int i = 0; i < systemInfo.dwNumberOfProcessors * 2; i++)
	{
		_beginthreadex(NULL, 0, _CompletionThread, (LPVOID)this, 0, NULL);
	}

	listenSocket = WSASocket(PF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
	if (listenSocket == INVALID_SOCKET)
	{
		throw runtime_error("WSASocket() error!");
	}

	serverAddr = {};
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_addr.s_addr = htonl(INADDR_ANY);
	serverAddr.sin_port = htons(PORT);

	if (bind(listenSocket, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR)
	{
		throw runtime_error("bind() error!");
	}

	if (listen(listenSocket, 10) == SOCKET_ERROR)
	{
		throw runtime_error("listen() error!");
	}
}

IOCPServer::~IOCPServer()
{
	for (auto socket : socketList) {
		closesocket(socket);
	}

	closesocket(listenSocket);
	WSACleanup();
}

bool IOCPServer::Run()
{
	LPPER_IO_DATA     perIoData;
	LPPER_HANDLE_DATA perHandleData;

	DWORD  dwRecvBytes;
	DWORD  dwFlags;

	while (true)
	{
		SOCKET      clientSocket;
		SOCKADDR_IN clientAddr;
		int         nAddrLen = sizeof(clientAddr);

		clientSocket = accept(listenSocket, (SOCKADDR*)&clientAddr, &nAddrLen);
		if (clientSocket == INVALID_SOCKET)
		{
			cout << "accept() error!" << endl;
			continue;
		}

		socketList.push_back(clientSocket);

		perHandleData = new PER_HANDLE_DATA;
		perHandleData->clientSocket = clientSocket;
		clientAddr = perHandleData->clntAddr;

		CreateIoCompletionPort((HANDLE)clientSocket, CompletionPortHandler, (DWORD)perHandleData, 0);

		perIoData = new PER_IO_DATA;
		perIoData->overlapped = {};
		perIoData->wsaBuf.len = BUFSIZE;
		perIoData->wsaBuf.buf = perIoData->buffer;
		perIoData->rwMode = READ;
		dwFlags = 0;

		WSARecv(perHandleData->clientSocket,       // 데이타 입력 소켓
			&(perIoData->wsaBuf),               // 데이타 입력 버퍼 포인터
			1,                                  // 데이타 입력 버퍼의 수
			&dwRecvBytes,
			&dwFlags,
			&(perIoData->overlapped),           // OVERLAPPED 변수 포인터
			NULL
		);
	}

	return true;
}


unsigned int __stdcall IOCPServer::_CompletionThread(void* p_this)
{
	IOCPServer* p_IOCPServer = static_cast<IOCPServer*>(p_this);
	p_IOCPServer->CompletionThread();
	return 0;
}

UINT WINAPI IOCPServer::CompletionThread()
{
	HANDLE hCompletionPort = (HANDLE)CompletionPortHandler;
	DWORD  dwBytesTransferred;
	LPPER_HANDLE_DATA perHandleData;
	LPPER_IO_DATA     perIoData;
	DWORD  dwFlags;

	while (TRUE)
	{
		GetQueuedCompletionStatus(hCompletionPort,
			&dwBytesTransferred,
			(LPDWORD)&perHandleData,
			(LPOVERLAPPED*)&perIoData,
			INFINITE);

		if (perIoData->rwMode == READ)
		{
			cout << "Recv Msg : " << perIoData->wsaBuf.buf << endl;
			if (dwBytesTransferred == 0)
			{
				cout << "socket will be closed!" << endl;
				closesocket(perHandleData->clientSocket);

				delete perHandleData;
				delete perIoData;

				continue;
			}

			for (auto socket : socketList) {
				perIoData->overlapped = {};
				perIoData->wsaBuf.len = dwBytesTransferred;
				perIoData->rwMode = WRITE;
				WSASend(socket, &(perIoData->wsaBuf), 1, &dwBytesTransferred, 0, &(perIoData->overlapped), NULL);
			}

			perIoData = new PER_IO_DATA();
			perIoData->overlapped = {};
			perIoData->wsaBuf.len = BUFSIZE;
			perIoData->wsaBuf.buf = perIoData->buffer;
			perIoData->rwMode = READ;

			dwFlags = 0;

			WSARecv(perHandleData->clientSocket, &(perIoData->wsaBuf), 1, NULL, &dwFlags, &(perIoData->overlapped), NULL);
		}
		else
		{
			cout << "message sent!" << endl;
		}
	}

	return 0;
}