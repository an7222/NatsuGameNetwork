#include "Wrapper.h"

Wrapper::Wrapper() : m_Type(Type::NONE)
{
	memset(&m_Data, 0, sizeof(m_Data));
}

Wrapper::~Wrapper()
{

}

Wrapper::Wrapper(char data) : m_Type(Type::CHAR)
{
	m_Data.charData = data;
}

Wrapper::Wrapper(short data) : m_Type(Type::SHORT)
{
	m_Data.shortData = data;
}

Wrapper::Wrapper(int data) : m_Type(Type::INT)
{
	m_Data.intData = data;
}

Wrapper::Wrapper(float data) : m_Type(Type::FLOAT)
{
	m_Data.floatData = data;
}

Wrapper::Wrapper(double data) : m_Type(Type::DOUBLE)
{
	m_Data.doubleData = data;
}

Wrapper::Wrapper(bool data) : m_Type(Type::BOOL)
{
	m_Data.boolData = data;
}

Wrapper::Wrapper(long long data) : m_Type(Type::LONGLONG)
{
	m_Data.longlongData = data;
}

Wrapper::Wrapper(const char* data) : m_Type(Type::STRING)
{
	m_Data.stringData = new (std::nothrow) std::string();

	if (data)
		*m_Data.stringData = data;
}

Wrapper::Wrapper(const string& data) : m_Type(Type::STRING)
{
	m_Data.stringData = new (std::nothrow) std::string();
	*m_Data.stringData = data;
}

Wrapper::Wrapper(const Wrapper& other) : m_Type(Type::NONE)
{
	*this = other;
}

Wrapper::Wrapper(Wrapper&& other) : m_Type(Type::NONE)
{
	*this = std::move(other);
}

bool Wrapper::operator!= (const Wrapper& data)
{
	return !(*this == data);
}

bool Wrapper::operator!= (const Wrapper& data) const
{
	return !(*this == data);
}

bool Wrapper::operator== (const Wrapper& data)
{
	const auto& t = *this;
	return t == data;
}

bool Wrapper::operator== (const Wrapper& data) const
{
	if (this == &data) return true;
	if (data.m_Type != this->m_Type) return false;
	if (this->isNull()) return true;
	switch (m_Type)
	{
	case Type::CHAR: return data.m_Data.charData == this->m_Data.charData;
	case Type::INT: return data.m_Data.intData == this->m_Data.intData;
	case Type::SHORT: return data.m_Data.shortData == this->m_Data.shortData;
	case Type::LONGLONG: return data.m_Data.longlongData == this->m_Data.longlongData;
	case Type::BOOL: return data.m_Data.boolData == this->m_Data.boolData;
	case Type::STRING: return *data.m_Data.stringData == *this->m_Data.stringData;
	case Type::FLOAT: return std::abs(data.m_Data.floatData - this->m_Data.floatData) <= FLT_EPSILON;
	case Type::DOUBLE: return std::abs(data.m_Data.doubleData - this->m_Data.doubleData) <= DBL_EPSILON;
	default:
		break;
	};

	return false;
}

Wrapper& Wrapper::operator=(const Wrapper& other)
{
	if (this != &other)
	{
		switch (other.m_Type)
		{
		case Type::CHAR:
			m_Data.charData = other.m_Data.charData;
			break;
		case Type::INT:
			m_Data.intData = other.m_Data.intData;
			break;
		case Type::LONGLONG:
			m_Data.longlongData = other.m_Data.longlongData;
			break;
		case Type::SHORT:
			m_Data.shortData = other.m_Data.shortData;
			break;
		case Type::FLOAT:
			m_Data.floatData = other.m_Data.floatData;
			break;
		case Type::DOUBLE:
			m_Data.doubleData = other.m_Data.doubleData;
			break;
		case Type::BOOL:
			m_Data.boolData = other.m_Data.boolData;
			break;
		case Type::STRING:
			if (m_Data.stringData == nullptr)
				m_Data.stringData = new std::string();

			*m_Data.stringData = *other.m_Data.stringData;
			break;
		default:
			break;
		}
	}

	return *this;
}

Wrapper& Wrapper::operator=(Wrapper&& other)
{
	if (this != &other)
	{
		switch (other.m_Type)
		{
		case Type::CHAR:
			m_Data.charData = other.m_Data.charData;
			break;
		case Type::INT:
			m_Data.intData = other.m_Data.intData;
			break;
		case Type::LONGLONG:
			m_Data.longlongData = other.m_Data.longlongData;
			break;
		case Type::SHORT:
			m_Data.shortData = other.m_Data.shortData;
			break;
		case Type::FLOAT:
			m_Data.floatData = other.m_Data.floatData;
			break;
		case Type::DOUBLE:
			m_Data.doubleData = other.m_Data.doubleData;
			break;
		case Type::BOOL:
			m_Data.boolData = other.m_Data.boolData;
			break;
		case Type::STRING:
			m_Data.stringData = other.m_Data.stringData;
			break;
		default:
			break;
		}

		m_Type = other.m_Type;
		memset(&other.m_Data, 0, sizeof(other.m_Data));
		other.m_Type = Type::NONE;
	}

	return *this;
}


char Wrapper::asChar() const
{
	if (m_Type == Type::CHAR)
	{
		return m_Data.charData;
	}

	if (m_Type == Type::INT)
	{
		return static_cast<char>(m_Data.intData);
	}

	if (m_Type == Type::SHORT)
	{
		return static_cast<char>(m_Data.shortData);
	}

	if (m_Type == Type::LONGLONG)
	{
		return static_cast<char>(m_Data.longlongData);
	}

	if (m_Type == Type::STRING)
	{
		return static_cast<char>(atoi(m_Data.stringData->c_str()));
	}

	if (m_Type == Type::FLOAT)
	{
		return static_cast<char>(m_Data.floatData);
	}

	if (m_Type == Type::DOUBLE)
	{
		return static_cast<char>(m_Data.doubleData);
	}

	if (m_Type == Type::BOOL)
	{
		return m_Data.boolData ? 1 : 0;
	}

	return 0;
}

int Wrapper::asInt() const
{
	if (m_Type == Type::INT)
	{
		return m_Data.intData;
	}

	if (m_Type == Type::SHORT)
	{
		return (int)m_Data.shortData;
	}

	if (m_Type == Type::SHORT)
	{
		return (int)m_Data.longlongData;
	}

	if (m_Type == Type::CHAR)
	{
		return m_Data.charData;
	}

	if (m_Type == Type::STRING)
	{
		return atoi(m_Data.stringData->c_str());
	}

	if (m_Type == Type::FLOAT)
	{
		return static_cast<int>(m_Data.floatData);
	}

	if (m_Type == Type::DOUBLE)
	{
		return static_cast<int>(m_Data.doubleData);
	}

	if (m_Type == Type::BOOL)
	{
		return m_Data.boolData ? 1 : 0;
	}

	return 0;
}


short Wrapper::asShort() const
{
	if (m_Type == Type::SHORT)
	{
		return m_Data.shortData;
	}

	if (m_Type == Type::LONGLONG)
	{
		return static_cast<short>(m_Data.longlongData);
	}

	if (m_Type == Type::INT)
	{
		return static_cast<short>(m_Data.intData);
	}

	if (m_Type == Type::CHAR)
	{
		return static_cast<short>(m_Data.charData);
	}

	if (m_Type == Type::STRING)
	{
		// NOTE: strtoul is required (need to augment on unsupported platforms)
		return static_cast<short>(strtoul(m_Data.stringData->c_str(), nullptr, 10));
	}

	if (m_Type == Type::FLOAT)
	{
		return static_cast<short>(m_Data.floatData);
	}

	if (m_Type == Type::DOUBLE)
	{
		return static_cast<short>(m_Data.doubleData);
	}

	if (m_Type == Type::BOOL)
	{
		return m_Data.boolData ? 1u : 0u;
	}

	return 0;
}

long long Wrapper::asLongLong() const
{
	if (m_Type == Type::LONGLONG)
	{
		return m_Data.longlongData;
	}

	if (m_Type == Type::INT)
	{
		return m_Data.intData;
	}

	if (m_Type == Type::SHORT)
	{
		return m_Data.shortData;
	}

	if (m_Type == Type::CHAR)
	{
		return static_cast<long long>(m_Data.charData);
	}

	if (m_Type == Type::STRING)
	{
		// NOTE: strtoul is required (need to augment on unsupported platforms)
		return static_cast<long long>(strtoul(m_Data.stringData->c_str(), nullptr, 10));
	}

	if (m_Type == Type::FLOAT)
	{
		return static_cast<long long>(m_Data.floatData);
	}

	if (m_Type == Type::DOUBLE)
	{
		return static_cast<long long>(m_Data.doubleData);
	}

	if (m_Type == Type::BOOL)
	{
		return m_Data.boolData ? 1u : 0u;
	}

	return 0ll;
}

float Wrapper::asFloat() const
{
	if (m_Type == Type::FLOAT)
	{
		return m_Data.floatData;
	}

	if (m_Type == Type::CHAR)
	{
		return static_cast<float>(m_Data.charData);
	}

	if (m_Type == Type::STRING)
	{
		return stof(m_Data.stringData->c_str());
	}

	if (m_Type == Type::INT)
	{
		return static_cast<float>(m_Data.intData);
	}

	if (m_Type == Type::SHORT)
	{
		return static_cast<float>(m_Data.shortData);
	}

	if (m_Type == Type::LONGLONG)
	{
		return static_cast<float>(m_Data.longlongData);
	}

	if (m_Type == Type::DOUBLE)
	{
		return static_cast<float>(m_Data.doubleData);
	}

	if (m_Type == Type::BOOL)
	{
		return m_Data.boolData ? 1.0f : 0.0f;
	}

	return 0.0f;
}

double Wrapper::asDouble() const
{
	if (m_Type == Type::DOUBLE)
	{
		return m_Data.doubleData;
	}

	if (m_Type == Type::CHAR)
	{
		return static_cast<double>(m_Data.charData);
	}

	if (m_Type == Type::STRING)
	{
		return static_cast<double>(stof(m_Data.stringData->c_str()));
	}

	if (m_Type == Type::INT)
	{
		return static_cast<double>(m_Data.intData);
	}

	if (m_Type == Type::SHORT)
	{
		return static_cast<double>(m_Data.shortData);
	}

	if (m_Type == Type::LONGLONG)
	{
		return static_cast<double>(m_Data.longlongData);
	}

	if (m_Type == Type::FLOAT)
	{
		return static_cast<double>(m_Data.floatData);
	}

	if (m_Type == Type::BOOL)
	{
		return m_Data.boolData ? 1.0 : 0.0;
	}

	return 0.0;
}

bool Wrapper::asBool() const
{
	if (m_Type == Type::BOOL)
	{
		return m_Data.boolData;
	}

	if (m_Type == Type::CHAR)
	{
		return m_Data.charData == 0 ? false : true;
	}

	if (m_Type == Type::STRING)
	{
		return (*m_Data.stringData == "0" || *m_Data.stringData == "false") ? false : true;
	}

	if (m_Type == Type::INT)
	{
		return m_Data.intData == 0 ? false : true;
	}

	if (m_Type == Type::SHORT)
	{
		return m_Data.shortData == 0 ? false : true;
	}

	if (m_Type == Type::LONGLONG)
	{
		return m_Data.longlongData == 0 ? false : true;
	}

	if (m_Type == Type::FLOAT)
	{
		return m_Data.floatData == 0.0f ? false : true;
	}

	if (m_Type == Type::DOUBLE)
	{
		return m_Data.doubleData == 0.0 ? false : true;
	}

	return false;
}

std::string Wrapper::asString() const
{
	if (m_Type == Type::STRING)
	{
		return *m_Data.stringData;
	}

	stringstream ret;

	switch (m_Type)
	{
	case Type::CHAR:
		ret << m_Data.charData;
		break;
	case Type::INT:
		ret << m_Data.intData;
		break;
	case Type::SHORT:
		ret << m_Data.shortData;
		break;
	case Type::LONGLONG:
		ret << m_Data.longlongData;
		break;
	case Type::FLOAT:
		ret << std::fixed << std::setprecision(7) << m_Data.floatData;
		break;
	case Type::DOUBLE:
		ret << std::fixed << std::setprecision(16) << m_Data.doubleData;
		break;
	case Type::BOOL:
		ret << (m_Data.boolData ? "true" : "false");
		break;
	default:
		break;
	}
	return ret.str();
}


Wrapper::Type Wrapper::getType()
{
	return m_Type;
}

void Wrapper::setType(const Type type)
{
	m_Type = type;
}

bool Wrapper::isNull() const
{
	return m_Type == Type::NONE;
}

void Wrapper::clear()
{
	switch (m_Type)
	{
	case Type::CHAR:
		m_Data.charData = 0;
		break;
	case Type::INT:
		m_Data.intData = 0;
		break;
	case Type::SHORT:
		m_Data.shortData = 0u;
		break;
	case Type::LONGLONG:
		m_Data.longlongData = 0ll;
		break;
	case Type::FLOAT:
		m_Data.floatData = 0.0f;
		break;
	case Type::DOUBLE:
		m_Data.doubleData = 0.0;
		break;
	case Type::BOOL:
		m_Data.boolData = false;
		break;
	case Type::STRING:
		delete m_Data.stringData;
		m_Data.stringData = nullptr;
		break;
	default:
		break;
	}

	m_Type = Type::NONE;
}

void Wrapper::reset(Type type)
{
	if (m_Type == type)
		return;

	clear();

	switch (type)
	{
	case Type::STRING:
		m_Data.stringData = new string();
		break;
	default:
		break;
	}

	m_Type = type;
}