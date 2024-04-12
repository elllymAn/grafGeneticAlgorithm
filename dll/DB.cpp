#include "pch.h"
#include "DB.h"

DB& DB::operator=(const DB& obj)
{
	if (this != &obj)
	{
		warehouse->clear();
		std::for_each(obj.warehouse->begin(), obj.warehouse->end(),
			[&](std::string obj)
			{
				warehouse->push_back(obj);
			});
	}
	return *this;
}

DB::DB(const DB& obj)
{
	warehouse = new std::vector <std::string>;
	std::for_each(obj.warehouse->begin(), obj.warehouse->end(),
		[&](std::string obj)
		{
			warehouse->push_back(obj);
		});
}

DB::DB(std::string d)
{
	warehouse = new std::vector <std::string>;
	std::istringstream ss(d);
	std::string token;
	while (std::getline(ss, token, '/'))
	{
		warehouse->push_back(token);
	}
}

void DB::returnLog(std::ofstream& file)
{
	std::for_each(warehouse->begin(), warehouse->end(),
		[&](std::string& obj)
		{
			file << obj << '\n';
		});
}

DB::~DB()
{
	delete warehouse;
}