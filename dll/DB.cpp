#include "pch.h"
#include "DB.h"

DB& DB::operator=(const DB& obj)
{
	if (this != &obj)
	{
		warehouse->clear();
		std::for_each(obj.warehouse->begin(), obj.warehouse->end(),
			[&](Graph obj)
			{
				warehouse->push_back(obj);
			});
	}
	return *this;
}

DB::DB(const DB& obj)
{
	warehouse = new std::vector <Graph>();
	std::for_each(obj.warehouse->begin(), obj.warehouse->end(),
		[&](Graph obj)
		{
			warehouse->push_back(obj);
		});
}
DB::DB(std::vector <Graph>& v)
{
	warehouse = new std::vector <Graph>();
	std::for_each(v.begin(), v.end(),
		[&](Graph obj)
		{
			warehouse->push_back(obj);
		});
}

void DB::returnLog(std::ofstream& file)
{
	std::for_each(warehouse->begin(), warehouse->end(),
		[&](Graph& obj)
		{
			file << "Граф:\n";
			std::for_each(obj.outInfo()->begin(), obj.outInfo()->end(),
				[&](Figure* elem)
				{
					elem->OutInfo(file);
				});
		});
}

DB::~DB()
{
	delete warehouse;
}
