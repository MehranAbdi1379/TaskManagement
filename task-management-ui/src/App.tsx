import { useEffect, useState } from "react";
import { Button, Text } from "@chakra-ui/react";
import { getTasks } from "./api/taskApi";

function App() {
  const [count, setCount] = useState(0);
  const [data, setData] = useState<
    [{ title: string; description: string; state: string }]
  >([{ title: "", description: "", state: "" }]);
  console.log(data);

  return (
    <>
      <div>
        <Text>{count}</Text>
        <Button
          onClick={() => {
            setCount(count + 3);
          }}
        >
          Much more
        </Button>
        <Button
          onClick={async () => {
            setData(await getTasks());
          }}
        >
          Send API
        </Button>
        {data.map((item, index) => (
          <Text>
            <Text>{index}</Text>
            <Text>{item.title}</Text>
            <Text>{item.state}</Text>
            <Text>{item.description}</Text>
          </Text>
        ))}
      </div>
    </>
  );
}

export default App;
