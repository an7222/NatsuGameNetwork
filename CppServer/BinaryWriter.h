#pragma once
#include <iostream>
#include <vector>
#include "common.h"
#include "Wrapper.h"

using namespace std;

class BinaryWriter
{
private:

	std::vector<std::pair<int, Wrapper>> m_VectorData;

	PROPERTY(int, m_FileSize, FileSize);

	PROPERTY(std::string, m_FilePath, FilePath);

public:

	BinaryWriter(const char* file);

	void writeBool(const bool& data);
	void writeLongLong(const long long& data);
	void writeInt(const int& data);
	void writeShort(const short& data);
	void writeDouble(const double& data);
	void writeFloat(const float& data);
	void writeString(const std::string& data);

	void open(const char* file);
	void clear();
	void close();
};