WITH RECURSIVE tree(id, chief_id, depth) AS (
    SELECT id, chief_id, 0
    FROM "employee" WHERE chief_id IS NULL
    UNION ALL
    SELECT employee.id, employee.chief_id, depth + 1
    FROM "employee" INNER JOIN tree on tree.id = employee.chief_id
)
SELECT depth FROM tree order by depth desc LIMIT 1;