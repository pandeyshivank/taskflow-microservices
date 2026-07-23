import { BrowserRouter } from "react-router-dom";
import TaskflowAppRoutes from "./routes/TaskFlowRoute";

function App() {
  return (
    <>
      <h1>Task Flow Project</h1>
      <BrowserRouter>
        <TaskflowAppRoutes></TaskflowAppRoutes>
      </BrowserRouter>
    </>
  );
}

export default App;
