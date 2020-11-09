#include "BinaryReader.h"

BinaryReader::BinaryReader()
{
	//empty
}

BinaryReader::BinaryReader(const char* file)
{
	open(file);
}

bool BinaryReader::open(const char* file)
{
	clear();

	ifstream infile(file);
	if (!infile.good())
	{
		return false;
	}

	m_Data = getBytesFromFile(file);

	m_Seek = 0;

	m_Buffer = reinterpret_cast<char*>(m_Data.data());

	m_FileSize = m_Data.size();

	return true;
}

bool BinaryReader::readBool()
{
	bool data = false;
	memcpy(&data, &m_Buffer[m_Seek], sizeof(bool));
	m_Seek += sizeof(bool);

	return data;
}

long long BinaryReader::readLongLong()
{
	long long data = -1LL;
	memcpy(&data, &m_Buffer[m_Seek], sizeof(long long));
	m_Seek += sizeof(long long);

	return data;
}

int BinaryReader::readInt()
{
	int data = -1;
	memcpy(&data, &m_Buffer[m_Seek], sizeof(int));
	m_Seek += sizeof(int);

	return data;
}

short BinaryReader::readShort()
{
	short data = -1;
	memcpy(&data, &m_Buffer[m_Seek], sizeof(short));
	m_Seek += sizeof(short);

	return data;
}

double BinaryReader::readDouble()
{
	double data = -1.0;
	memcpy(&data, &m_Buffer[m_Seek], sizeof(double));
	m_Seek += sizeof(double);

	return data;
}

float BinaryReader::readFloat()
{
	float data = -1.0F;
	memcpy(&data, &m_Buffer[m_Seek], sizeof(float));
	m_Seek += sizeof(float);

	return data;
}

std::string BinaryReader::readString()
{
	string data = "";

	short length = 0;
	memcpy(&length, &m_Buffer[m_Seek], sizeof(short));
	m_Seek += sizeof(length);

	if (length > 0)
	{
		data.resize(length);
		memcpy(&data.at(0), &m_Buffer[m_Seek], sizeof(char) * length);
		m_Seek += sizeof(char) * length;
	}

	return data;
}

void BinaryReader::clear()
{
	m_Seek = 0;
	m_FileSize = 0;
	m_Buffer = nullptr;
}

bool BinaryReader::hasNext()
{
	return (m_Seek < m_FileSize);
}

vector<char> BinaryReader::getBytesFromFile(const char* file)
{
	ifstream ifs(file, ios::binary | ios::ate);
	ifstream::pos_type pos = ifs.tellg();

	std::vector<char>  result(pos);

	ifs.seekg(0, ios::beg);
	ifs.read(&result[0], pos);

	return result;
}