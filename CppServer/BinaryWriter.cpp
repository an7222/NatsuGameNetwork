#include "BinaryWriter.h"

BinaryWriter::BinaryWriter(const char* file)
{
	open(file);
}

void BinaryWriter::writeBool(const bool& data)
{
	m_VectorData.push_back(std::pair<int, Wrapper>(sizeof(bool), Wrapper(data)));
	m_FileSize += sizeof(bool);
}

void BinaryWriter::writeLongLong(const long long& data)
{
	m_VectorData.push_back(std::pair<int, Wrapper>(sizeof(long long), Wrapper(data)));
	m_FileSize += sizeof(long long);
}

void BinaryWriter::writeInt(const int& data)
{
	m_VectorData.push_back(std::pair<int, Wrapper>(sizeof(int), Wrapper(data)));
	m_FileSize += sizeof(int);
}

void BinaryWriter::writeShort(const short& data)
{
	m_VectorData.push_back(std::pair<int, Wrapper>(sizeof(short), Wrapper(data)));
	m_FileSize += sizeof(short);
}

void BinaryWriter::writeDouble(const double& data)
{
	m_VectorData.push_back(std::pair<int, Wrapper>(sizeof(double), Wrapper(data)));
	m_FileSize += sizeof(double);
}

void BinaryWriter::writeFloat(const float& data)
{
	m_VectorData.push_back(std::pair<int, Wrapper>(sizeof(float), Wrapper(data)));
	m_FileSize += sizeof(float);
}

void BinaryWriter::writeString(const std::string& data)
{
	writeShort(static_cast<const short>(data.size()));

	m_VectorData.push_back(std::pair<int, Wrapper>((sizeof(char) * data.size()), Wrapper(data)));
	m_FileSize += sizeof(char) * data.size();
}

void BinaryWriter::open(const char* file)
{
	clear();
	m_FilePath = file;
}

void BinaryWriter::clear()
{
	m_VectorData.clear();
	m_FileSize = 0;
	m_FilePath = "";
}

void BinaryWriter::close()
{
	//close할때 파일에 데이터를 쓴다. 반드시 마지막에 close 해야한다.
	int seek = 0;
	char* buf = (char*)(malloc(m_FileSize));
	memset(buf, 0, m_FileSize);

	for (unsigned int i = 0; i < m_VectorData.size(); i++)
	{
		pair<int, Wrapper>& data = m_VectorData.at(i);

		int dataSize = data.first;
		Wrapper::Type dataType = data.second.getType();

		if (dataType == Wrapper::Type::INT)
		{
			if (dataSize == sizeof(int))
			{
				int value = data.second.asInt();
				memcpy(&buf[seek], &value, dataSize);
			}
			else //short
			{
				short value = data.second.asInt();
				memcpy(&buf[seek], &value, dataSize);
			}

			seek += dataSize;
		}
		else if (dataType == Wrapper::Type::SHORT)
		{
			short value = data.second.asInt();
			memcpy(&buf[seek], &value, dataSize);
			seek += dataSize;
		}
		else if (dataType == Wrapper::Type::LONGLONG)
		{
			long long value = data.second.asLongLong();
			memcpy(&buf[seek], &value, dataSize);
			seek += dataSize;
		}
		else if (dataType == Wrapper::Type::BOOL)
		{
			bool value = data.second.asBool();
			memcpy(&buf[seek], &value, dataSize);
			seek += dataSize;
		}
		else if (dataType == Wrapper::Type::FLOAT)
		{
			float value = data.second.asFloat();
			memcpy(&buf[seek], &value, dataSize);
			seek += dataSize;
		}
		else if (dataType == Wrapper::Type::DOUBLE)
		{
			double value = data.second.asDouble();
			memcpy(&buf[seek], &value, dataSize);
			seek += dataSize;
		}
		else if (dataType == Wrapper::Type::STRING)
		{
			if (dataSize > 0)
			{
				std::string value = data.second.asString();

				memcpy(&buf[seek], &value.at(0), dataSize);
				seek += dataSize;
			}
		}
	}

	//FILE* fp = fopen(m_FilePath.c_str(), "wb");
	//fwrite(buf, sizeof(char), m_FileSize, fp);
	//fclose(fp);

	free(buf);

	return;
}