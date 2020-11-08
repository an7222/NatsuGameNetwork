#include "IOCPServer.h"
//MemoryLeak Debug
//#define _CRTDBG_MAP_ALLOC
//#include <crtdbg.h>
//#include <Windows.h>

int main()
{
    IOCPServer* server = new IOCPServer();

    //_CrtSetBreakAlloc(201);

    try {
        server->Run();
    }
    catch (std::exception& e) {
        std::cout << "Exception : " << e.what();
    }

    //_CrtDumpMemoryLeaks();

    return 0;
}