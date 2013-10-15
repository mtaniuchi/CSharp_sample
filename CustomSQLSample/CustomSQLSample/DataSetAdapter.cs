using System.Collections.Generic;
using System.Collections.ObjectModel;
using CustomSQLSample.Database1DataSetTableAdapters;

namespace CustomSQLSample
{
    public class DataSetAdapter
    {
        private TableAdapterManager manager { get; set; }

        public DataSetAdapter()
        {
            this.manager = new TableAdapterManager
            {
                StatesTableAdapter = new StatesTableAdapter()
            };
        }

        #region public methods
        /// <summary>
        /// get all states
        /// </summary>
        /// <returns>all states</returns>
        public ICollection<State> getAll()
        {
            var dataset = new Database1DataSet { EnforceConstraints = false };

            dataset.BeginInit();
            this.manager.StatesTableAdapter.Fill(dataset.States);
            dataset.EndInit();

            var result = prepareStates(dataset.States);

            dataset.Dispose();

            return result;
        }

        /// <summary>
        /// get states with IN operator
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public ICollection<State> getWithInOperator(IEnumerable<string> names)
        {
            var dataset = new Database1DataSet { EnforceConstraints = false };

            dataset.BeginInit();
            this.manager.StatesTableAdapter.FillByInOperator(dataset.States, names);
            dataset.EndInit();

            var result = prepareStates(dataset.States);

            dataset.Dispose();

            return result;
        }
        #endregion public methods



        #region private methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statesDataTable"></param>
        /// <returns></returns>
        private static Collection<State> prepareStates(IEnumerable<Database1DataSet.StatesRow> statesDataTable)
        {
            var result = new Collection<State>();
            foreach (var row in statesDataTable)
            {
                result.Add(
                    new State
                    {
                        Capital = (row.IsCapitalNull() ? string.Empty : row.Capital),
                        Metropolis = (row.IsMetropolisNull() ? string.Empty : row.Metropolis),
                        Name = row.Name,
                        Nicname = (row.IsNicnameNull() ? string.Empty : row.Nicname)
                    });
            }
            return result;
        }
        #endregion private methods

    }
}
