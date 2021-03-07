using System;
using System.Diagnostics;
using Benchmark.Model;
using Model.Mapper;

namespace Benchmark {
    internal class Benchmarker {
        private readonly IModelMapper _mapper;
        private readonly MasterClassSource _source;
        private readonly MasterClassTarget _target;

        public Benchmarker() {
            _mapper = new ModelMapper();
            _source = new MasterClassSource();
            _target = new MasterClassTarget();
        }

        public void Start() {
            Console.WriteLine("Discarding first 100 operations...");
            for (var i = 0; i < 100; i++) {
                _mapper.Set(_target).From(_source);
            }

            Console.WriteLine("Starting profile of 1.000.000 operations...");

            var timer = Stopwatch.StartNew();
            for (var i = 0; i < 1000000; i++) {
                _mapper.Set(_target).From(_source);
            }
            timer.Stop();
            
            Console.WriteLine("Result:");
            Console.WriteLine($"Total mapping time: \t{timer.Elapsed.TotalMilliseconds}ms.");
            Console.WriteLine($"Average mapping time: \t{timer.Elapsed.TotalMilliseconds / 11000.0}µs per operation.");
        }
    }
}