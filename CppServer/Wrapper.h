#pragma once
#include <string>
#include <iostream>
#include <sstream>
#include <iomanip>

using namespace std;

class Wrapper
{
public:
	enum class Type
	{
		NONE, CHAR, INT, SHORT, LONGLONG, FLOAT, DOUBLE, BOOL, STRING
	};

private:

	union Data
	{
		char charData;
		int intData;
		long long longlongData;
		short shortData;
		float floatData;
		double doubleData;
		bool boolData;
		std::string* stringData;
	};

private:
	Type m_Type;
	Data m_Data;
	string* strData;

public:
	Wrapper();
	~Wrapper();

	Wrapper(char data);
	Wrapper(short data);
	Wrapper(int data);
	Wrapper(float data);
	Wrapper(double data);
	Wrapper(bool data);
	Wrapper(long long data);
	Wrapper(const char* data);
	Wrapper(const string& data);
	Wrapper(const Wrapper& other);
	Wrapper(Wrapper&& other);

	bool operator!= (const Wrapper& data);
	bool operator!= (const Wrapper& data) const;
	bool operator== (const Wrapper& data);
	bool operator== (const Wrapper& data) const;

	Wrapper& operator=(const Wrapper& other);
	Wrapper& operator=(Wrapper&& other);

	char asChar() const;
	int asInt() const;
	short asShort() const;
	long long asLongLong() const;
	float asFloat() const;
	double asDouble() const;
	bool asBool() const;
	std::string asString() const;

	Type getType();

	void setType(const Type type);

	bool isNull() const;

private:
	void clear();
	void reset(Type type);
};