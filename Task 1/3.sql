SELECT * FROM "department" WHERE id = (
    SELECT department_id
    FROM "department"
    INNER JOIN employee ON department.id = employee.department_id
    GROUP BY department_id
    HAVING SUM(salary) = (
        SELECT MAX(s.sum) FROM (
            SELECT SUM(salary) AS sum
            FROM "department"
            INNER JOIN employee ON department.id = employee.department_id
            GROUP BY department_id
        ) AS s
    ) LIMIT 1
);
