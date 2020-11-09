#pragma once
#include <string>
#include <fstream>
#include <vector>
#include "common.h"
using namespace std;

class BinaryReader
{
private:
	vector<char> m_Data;

	PROPERTY(int, m_Seek, Seek);

	PROPERTY(int, m_FileSize, FileSize);

	PROPERTY(char*, m_Buffer, Buffer);

public:
	BinaryReader();
	BinaryReader(const char* file);
	bool open(const char* file);

	bool readBool();
	long long readLongLong();
	int readInt();
	short readShort();
	double readDouble();
	float readFloat();
	std::string readString();

	void clear();
	bool hasNext();

private:
	vector<char> getBytesFromFile(const char* file);
};